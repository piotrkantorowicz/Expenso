using System.Reflection;
using System.Text.Json.Serialization;

using Expenso.Api.Configuration.Builders.Interfaces;
using Expenso.Api.Configuration.Configurators;
using Expenso.Api.Configuration.Configurators.Interfaces;
using Expenso.Api.Configuration.Errors;
using Expenso.Api.Configuration.Execution;
using Expenso.Api.Configuration.Extensions;
using Expenso.Api.Configuration.Extensions.Environment;
using Expenso.Api.Configuration.Settings;
using Expenso.IAM.Core.Acl.Keycloak;
using Expenso.Shared.Commands;
using Expenso.Shared.Commands.Logging;
using Expenso.Shared.Commands.Transactions;
using Expenso.Shared.Commands.Validation;
using Expenso.Shared.Domain.Events;
using Expenso.Shared.Domain.Events.Logging;
using Expenso.Shared.Integration.Events.Logging;
using Expenso.Shared.Integration.MessageBroker;
using Expenso.Shared.Queries;
using Expenso.Shared.Queries.Logging;
using Expenso.Shared.Api.ProblemDetails.Details;
using Expenso.Shared.System.Configuration.Sections;
using Expenso.Shared.System.Configuration.Settings.Auth;
using Expenso.Shared.System.Logging;
using Expenso.Shared.System.Logging.Serilog;
using Expenso.Shared.System.Metrics;
using Expenso.Shared.System.Modules;
using Expenso.Shared.System.Serialization;
using Expenso.Shared.System.Types;
using Expenso.Shared.System.Types.ExecutionContext;

using Keycloak.AuthServices.Authentication;
using Keycloak.AuthServices.Authorization;
using Keycloak.AuthServices.Sdk;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.OpenApi.Extensions;
using Microsoft.OpenApi.Models;

using JsonOptions = Microsoft.AspNetCore.Http.Json.JsonOptions;

namespace Expenso.Api.Configuration.Builders;

internal sealed class AppBuilder : IAppBuilder
{
    private readonly IAppConfigurationManager _appConfigurationManager;
    private readonly WebApplicationBuilder _applicationBuilder;
    private readonly IConfiguration _configuration;
    private readonly IServiceCollection _services;

    public AppBuilder(WebApplicationBuilder appBuilder, IConfiguration configuration,
        IServiceCollection serviceCollection, IAppConfigurationManager appConfigurationManager)
    {
        _appConfigurationManager = appConfigurationManager;
        _configuration = configuration;
        _services = serviceCollection;
        _applicationBuilder = appBuilder;
    }

    public IAppBuilder ConfigureApiDependencies()
    {
        _services.AddHttpContextAccessor();
        _services.AddSingleton(implementationInstance: _appConfigurationManager);
        _services.AddScoped<IExecutionContextAccessor, ExecutionContextAccessor>();
        _services.AddRouting(configureOptions: options => options.LowercaseUrls = true);

        return this;
    }

    public IAppBuilder ConfigureModules()
    {
        _services.AddModules(configuration: _configuration);

        return this;
    }

    public IAppBuilder ConfigureSharedFramework()
    {
        IReadOnlyCollection<Assembly> assemblies = Modules.GetRequiredModulesAssemblies();

        OtlpSettings otlpSettings =
            _appConfigurationManager.GetRequiredSettings<OtlpSettings>(sectionName: SectionNames.Otlp);

        _applicationBuilder.Host.AddSerilogLogger(otlpEndpoint: otlpSettings.Endpoint,
            otlpService: otlpSettings.ServiceName);

        _services
            .AddCommands(assemblies: assemblies)
            .AddCommandsValidations(assemblies: assemblies)
            .AddCommandsTransactions()
            .AddCommandsLogging()
            .AddQueries(assemblies: assemblies)
            .AddQueryLogging()
            .AddDomainEvents(assemblies: assemblies)
            .AddDomainEventsLogging()
            .AddIntegrationEvents(assemblies: assemblies)
            .AddIntegrationEventsLogging()
            .AddMessageBroker()
            .AddClock()
            .AddMessageContext()
            .AddDefaultSerializer()
            .AddInternalLogging()
            .AddOtlpMetrics(otlpSettings: otlpSettings);

        return this;
    }

    public IAppBuilder ConfigureHealthChecks()
    {
        _services.AddHealthChecks();

        return this;
    }

    public IAppBuilder ConfigureMvc()
    {
        _services.AddMvcCore();
        _services.AddEndpointsApiExplorer();
        _services.AddExceptionHandler<GlobalExceptionHandler>();
        _services.AddProblemDetails();

        return this;
    }

    public IAppBuilder ConfigureCors()
    {
        CorsSettings corsSettings =
            _appConfigurationManager.GetRequiredSettings<CorsSettings>(sectionName: SectionNames.Cors);

        if (corsSettings.Enabled is true)
        {
            _services.AddCors(setupAction: options => options.AddDefaultPolicy(configurePolicy: builder =>
            {
                CorsPolicyBuilder corsPolicyBuilder = builder.AllowAnyHeader().AllowAnyMethod().AllowCredentials();

                if (!_applicationBuilder.Environment.IsDevelopment() && !_applicationBuilder.Environment.IsLocal() &&
                    !_applicationBuilder.Environment.IsTest())
                {
                    corsPolicyBuilder.WithOrigins(origins: corsSettings.AllowedOrigins!);
                }
            }));
        }

        return this;
    }

    public IAppBuilder ConfigureCache()
    {
        _services.AddDistributedMemoryCache();

        return this;
    }

    public IAppBuilder ConfigureSerializationOptions()
    {
        _services.Configure<JsonOptions>(configureOptions: options =>
        {
            options.SerializerOptions.Converters.Add(item: new JsonStringEnumConverter());
        });

        return this;
    }

    public IAppBuilder ConfigureAuthorization()
    {
        const string tokenHandlerClient = "admin";
        _services.AddAuthorization();
        _services.AddAuthentication();

        KeycloakSettings keycloakSettings =
            _appConfigurationManager.GetRequiredSettings<KeycloakSettings>(sectionName: SectionNames.Keycloak);

        _services
            .AddClientCredentialsTokenManagement()
            .AddClient(name: tokenHandlerClient, configureOptions: client =>
            {
                client.ClientId = keycloakSettings.Resource;
                client.ClientSecret = keycloakSettings.Credentials.Secret;
                client.TokenEndpoint = keycloakSettings.KeycloakTokenEndpoint;
            });

        AuthSettings authSettings =
            _appConfigurationManager.GetRequiredSettings<AuthSettings>(sectionName: SectionNames.Auth);

        switch (authSettings.AuthServer)
        {
            case AuthServer.Keycloak:
                ConfigureKeycloak(tokenHandlerClient: tokenHandlerClient);

                break;
            default:
                throw new ArgumentOutOfRangeException(paramName: authSettings.AuthServer.GetType().Name,
                    actualValue: authSettings.AuthServer, message: "Invalid auth server type");
        }

        return this;
    }

    public IAppBuilder ConfigureSwagger()
    {
        _services.AddSwaggerGen(setupAction: options =>
        {
            options.SwaggerDoc(name: "v1", info: new OpenApiInfo
            {
                Title = "Expenso",
                Version = "v1"
            });

            KeycloakSettings keycloakOptions =
                _appConfigurationManager.GetRequiredSettings<KeycloakSettings>(sectionName: SectionNames.Keycloak);

            string schemeName = SecuritySchemeType.OpenIdConnect.GetDisplayName();

            options.AddSecurityDefinition(name: schemeName, securityScheme: new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.OpenIdConnect,
                Scheme = JwtBearerDefaults.AuthenticationScheme,
                OpenIdConnectUrl =
                    new Uri(
                        uriString:
                        $"{keycloakOptions.AuthServerUrl}/realms/{keycloakOptions.Realm}/.well-known/openid-configuration")
            });

            OpenApiSecurityScheme keycloakSecurityScheme = new()
            {
                Reference = new OpenApiReference
                {
                    Id = schemeName,
                    Type = ReferenceType.SecurityScheme
                },
                In = ParameterLocation.Header,
                Name = "Bearer",
                Scheme = "Bearer"
            };

            options.AddSecurityRequirement(securityRequirement: new OpenApiSecurityRequirement
            {
                { keycloakSecurityScheme, Array.Empty<string>() }
            });
        });

        return this;
    }

    public IAppConfigurator Build()
    {
        WebApplication app = _applicationBuilder.Build();

        return new AppConfigurator(app: app);
    }

    private void ConfigureKeycloak(string tokenHandlerClient, string? keyCloakAdminSection = null)
    {
        _services
            .AddAuthentication(defaultScheme: JwtBearerDefaults.AuthenticationScheme)
            .AddKeycloakWebApi(configuration: _configuration, configureJwtBearerOptions: jwtOptions =>
            {
                jwtOptions.Events = new JwtBearerEvents
                {
                    OnChallenge = async context =>
                    {
                        context.HandleResponse();
                        HttpContext httpContext = context.HttpContext;
                        RouteData routeData = httpContext.GetRouteData();

                        ActionContext actionContext = new(httpContext: httpContext, routeData: routeData,
                            actionDescriptor: new ActionDescriptor());

                        ObjectResult result = new(value: new UnauthorizedAccessProblemDetails())
                        {
                            StatusCode = StatusCodes.Status401Unauthorized
                        };

                        await result.ExecuteResultAsync(context: actionContext);
                    },
                    OnForbidden = async context =>
                    {
                        HttpContext httpContext = context.HttpContext;
                        RouteData routeData = httpContext.GetRouteData();

                        ActionContext actionContext = new(httpContext: httpContext, routeData: routeData,
                            actionDescriptor: new ActionDescriptor());

                        ObjectResult result = new(value: new ForbiddenProblemDetails())
                        {
                            StatusCode = StatusCodes.Status403Forbidden
                        };

                        await result.ExecuteResultAsync(context: actionContext);
                    }
                };
            });

        _services.AddKeycloakAuthorization().AddAuthorizationServer(configuration: _configuration);

        _services
            .AddKeycloakAdminHttpClient(configuration: _configuration,
                keycloakClientSectionName: keyCloakAdminSection ?? KeycloakAdminClientOptions.Section)
            .AddClientCredentialsTokenHandler(tokenClientName: tokenHandlerClient);
    }
}