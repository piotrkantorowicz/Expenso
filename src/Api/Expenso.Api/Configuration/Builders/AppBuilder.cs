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
using Expenso.Api.Configuration.Settings.Services.Containers;
using Expenso.BudgetSharing.Api;
using Expenso.Communication.Api;
using Expenso.DocumentManagement.Api;
using Expenso.IAM.Api;
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
using Expenso.Shared.System.Configuration.Sections;
using Expenso.Shared.System.Configuration.Settings.Auth;
using Expenso.Shared.System.Logging;
using Expenso.Shared.System.Logging.Serilog;
using Expenso.Shared.System.Metrics;
using Expenso.Shared.System.Modules;
using Expenso.Shared.System.Serialization;
using Expenso.Shared.System.Types;
using Expenso.Shared.System.Types.ExecutionContext;
using Expenso.TimeManagement.Api;
using Expenso.UserPreferences.Api;

using Keycloak.AuthServices.Authentication;
using Keycloak.AuthServices.Authorization;
using Keycloak.AuthServices.Sdk;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.OpenApi.Models;

using JsonOptions = Microsoft.AspNetCore.Http.Json.JsonOptions;

namespace Expenso.Api.Configuration.Builders;

internal sealed class AppBuilder : IAppBuilder
{
    private readonly WebApplicationBuilder _applicationBuilder;
    private readonly IConfiguration _configuration;
    private readonly IServiceCollection _services;
    private AppConfigurationManager? _appConfigurationManager;

    public AppBuilder(WebApplicationBuilder appBuilder)
    {
        _applicationBuilder = appBuilder;
        _configuration = appBuilder.Configuration;
        _services = appBuilder.Services;
    }

    public IAppBuilder ConfigureApiDependencies()
    {
        ConfigureAppSettings();
        _services.AddHttpContextAccessor();
        _services.AddScoped<IExecutionContextAccessor, ExecutionContextAccessor>();
        _services.AddRouting(configureOptions: options => options.LowercaseUrls = true);

        return this;
    }

    public IAppBuilder ConfigureModules()
    {
        Modules.RegisterModule<IamModule>();
        Modules.RegisterModule<UserPreferencesModule>();
        Modules.RegisterModule<BudgetSharingModule>();
        Modules.RegisterModule<DocumentManagementModule>();
        Modules.RegisterModule<CommunicationModule>();
        Modules.RegisterModule<TimeManagementModule>();
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
        OpenApiSecurityScheme securityScheme = new()
        {
            Name = "JWT Authentication",
            Description = "Enter JWT Bearer token **_only_**",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT",
            Reference = new OpenApiReference
            {
                Type = ReferenceType.SecurityScheme,
                Id = JwtBearerDefaults.AuthenticationScheme
            }
        };

        _services.AddSwaggerGen(setupAction: o =>
        {
            o.AddSecurityDefinition(name: securityScheme.Reference.Id, securityScheme: securityScheme);

            o.AddSecurityRequirement(securityRequirement: new OpenApiSecurityRequirement
            {
                { securityScheme, Array.Empty<string>() }
            });
        });

        return this;
    }

    public IAppConfigurator Build()
    {
        WebApplication app = _applicationBuilder.Build();

        return new AppConfigurator(app: app);
    }

    private void ConfigureAppSettings()
    {
        IPreStartupContainer preStartupContainer = new PreStartupContainer();

        preStartupContainer.Build(configuration: _configuration,
            assemblies: Modules.GetRequiredModulesAssemblies(merge: [typeof(Program).Assembly]));

        _appConfigurationManager = new AppConfigurationManager(preStartupContainer: preStartupContainer);
        _appConfigurationManager.Configure(serviceCollection: _services);
        _services.AddSingleton(implementationInstance: _appConfigurationManager);
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
                        const int statusCode = StatusCodes.Status401Unauthorized;
                        HttpContext httpContext = context.HttpContext;
                        RouteData routeData = httpContext.GetRouteData();

                        ActionContext actionContext = new(httpContext: httpContext, routeData: routeData,
                            actionDescriptor: new ActionDescriptor());

                        ProblemDetails problemDetails = new()
                        {
                            Status = StatusCodes.Status401Unauthorized,
                            Title = "Unauthorized",
                            Type = "https://tools.ietf.org/html/rfc7235#section-3.1"
                        };

                        ObjectResult result = new(value: problemDetails)
                        {
                            StatusCode = statusCode
                        };

                        await result.ExecuteResultAsync(context: actionContext);
                    },
                    OnForbidden = async context =>
                    {
                        const int statusCode = StatusCodes.Status403Forbidden;
                        HttpContext httpContext = context.HttpContext;
                        RouteData routeData = httpContext.GetRouteData();

                        ActionContext actionContext = new(httpContext: httpContext, routeData: routeData,
                            actionDescriptor: new ActionDescriptor());

                        ProblemDetails problemDetails = new()
                        {
                            Status = StatusCodes.Status403Forbidden,
                            Title = "Forbidden",
                            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.3"
                        };

                        ObjectResult result = new(value: problemDetails)
                        {
                            StatusCode = statusCode
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