using System.Reflection;
using System.Text.Json.Serialization;

using Expenso.Api.Configuration.Builders.Interfaces;
using Expenso.Api.Configuration.Errors;
using Expenso.Api.Configuration.Execution;
using Expenso.BudgetSharing.Api;
using Expenso.Communication.Api;
using Expenso.Communication.Proxy.DTO.Settings;
using Expenso.DocumentManagement.Api;
using Expenso.IAM.Api;
using Expenso.Shared.Commands;
using Expenso.Shared.Commands.Logging;
using Expenso.Shared.Commands.Transactions;
using Expenso.Shared.Commands.Validation;
using Expenso.Shared.Database.EfCore.Settings;
using Expenso.Shared.Domain.Events;
using Expenso.Shared.Domain.Events.Logging;
using Expenso.Shared.Integration.Events.Logging;
using Expenso.Shared.Integration.MessageBroker;
using Expenso.Shared.Queries;
using Expenso.Shared.Queries.Logging;
using Expenso.Shared.System.Configuration.Extensions;
using Expenso.Shared.System.Configuration.Sections;
using Expenso.Shared.System.Configuration.Settings;
using Expenso.Shared.System.Configuration.Settings.Auth;
using Expenso.Shared.System.Configuration.Settings.Files;
using Expenso.Shared.System.Logging;
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
    private ApplicationSettings? _applicationSettings;
    private AuthSettings? _authSettings;
    private EfCoreSettings? _efCoreSettings;
    private FilesSettings? _filesSettings;
    private KeycloakProtectionClientOptions? _keycloakProtectionClientOptions;
    private NotificationSettings? _notificationSettings;

    public AppBuilder(string[] args)
    {
        _applicationBuilder = WebApplication.CreateBuilder(args: args);
        _configuration = _applicationBuilder.Configuration;
        _services = _applicationBuilder.Services;
    }

    public IAppBuilder ConfigureApiDependencies()
    {
        ConfigureAppSettings();
        _services.AddHttpContextAccessor();
        _services.AddScoped<IExecutionContextAccessor, ExecutionContextAccessor>();

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
            .AddInternalLogging();

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

        _services
            .AddClientCredentialsTokenManagement()
            .AddClient(name: tokenHandlerClient, configureOptions: client =>
            {
                client.ClientId = _keycloakProtectionClientOptions?.Resource;
                client.ClientSecret = _keycloakProtectionClientOptions?.Credentials.Secret;
                client.TokenEndpoint = _keycloakProtectionClientOptions?.KeycloakTokenEndpoint;
            });

        switch (_authSettings?.AuthServer)
        {
            case AuthServer.Keycloak:
                ConfigureKeycloak(tokenHandlerClient: tokenHandlerClient);

                break;
            default:
                throw new ArgumentOutOfRangeException(paramName: _authSettings?.AuthServer.GetType().Name,
                    actualValue: _authSettings?.AuthServer, message: "Invalid auth server type");
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
        _configuration.TryBindOptions(sectionName: KeycloakProtectionClientOptions.Section,
            options: out _keycloakProtectionClientOptions);

        _configuration.TryBindOptions(sectionName: SectionNames.EfCoreSection, options: out _efCoreSettings);
        _configuration.TryBindOptions(sectionName: SectionNames.ApplicationSection, options: out _applicationSettings);
        _configuration.TryBindOptions(sectionName: SectionNames.Auth, options: out _authSettings);
        _configuration.TryBindOptions(sectionName: SectionNames.Files, options: out _filesSettings);
        _configuration.TryBindOptions(sectionName: SectionNames.Notifications, options: out _notificationSettings);
        _services.AddSingleton(implementationInstance: _efCoreSettings!);
        _services.AddSingleton(implementationInstance: _applicationSettings!);
        _services.AddSingleton(implementationInstance: _keycloakProtectionClientOptions!);
        _services.AddSingleton(implementationInstance: _authSettings!);
        _services.AddSingleton(implementationInstance: _filesSettings!);
        _services.AddSingleton(implementationInstance: _notificationSettings!);
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