using System.Reflection;
using System.Text.Json.Serialization;

using Expenso.Api.Configuration.Builders.Interfaces;
using Expenso.Api.Configuration.Errors;
using Expenso.Api.Configuration.Execution;
using Expenso.BudgetSharing.Api;
using Expenso.IAM.Api;
using Expenso.Shared.Commands;
using Expenso.Shared.Commands.Transactions;
using Expenso.Shared.Commands.Validation;
using Expenso.Shared.Database.EfCore;
using Expenso.Shared.Domain.Events;
using Expenso.Shared.Integration.MessageBroker;
using Expenso.Shared.Queries;
using Expenso.Shared.System.Configuration.Extensions;
using Expenso.Shared.System.Configuration.Sections;
using Expenso.Shared.System.Configuration.Settings;
using Expenso.Shared.System.Modules;
using Expenso.Shared.System.Types;
using Expenso.Shared.System.Types.ExecutionContext;
using Expenso.UserPreferences.Api;

using Keycloak.AuthServices.Authentication;
using Keycloak.AuthServices.Authorization;
using Keycloak.AuthServices.Sdk.Admin;

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
    private EfCoreSettings? _efCoreSettings;
    private KeycloakProtectionClientOptions? _keycloakProtectionClientOptions;

    public AppBuilder(string[] args)
    {
        _applicationBuilder = WebApplication.CreateBuilder(args);
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
        _services.AddModules(_configuration);

        return this;
    }

    public IAppBuilder ConfigureSharedFramework()
    {
        IReadOnlyCollection<Assembly> assemblies = Modules.GetRequiredModulesAssemblies();

        _services
            .AddCommands(assemblies)
            .AddCommandsValidations(assemblies)
            .AddCommandsTransactions()
            .AddQueries(assemblies)
            .AddDomainEvents(assemblies)
            .AddMessageBroker(assemblies)
            .AddClock();

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

    public IAppBuilder ConfigureSerializationOptions()
    {
        _services.Configure<JsonOptions>(options =>
        {
            options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
        });

        return this;
    }

    public IAppBuilder ConfigureAuthorization()
    {
        _services.AddAuthorization();
        _services.AddAuthentication();

        switch (_applicationSettings?.AuthServer)
        {
            case AuthServer.Keycloak:
                ConfigureKeycloak();

                break;
            default:
                throw new ArgumentOutOfRangeException(_applicationSettings?.AuthServer.GetType().Name,
                    _applicationSettings?.AuthServer, "Invalid auth server type.");
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

        _services.AddSwaggerGen(o =>
        {
            o.AddSecurityDefinition(securityScheme.Reference.Id, securityScheme);

            o.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                { securityScheme, Array.Empty<string>() }
            });
        });

        return this;
    }

    public IAppConfigurator Build()
    {
        WebApplication app = _applicationBuilder.Build();

        return new AppConfigurator(app);
    }

    private void ConfigureAppSettings()
    {
        _configuration.TryBindOptions(KeycloakProtectionClientOptions.Section, out _keycloakProtectionClientOptions);
        _configuration.TryBindOptions(SectionNames.EfCoreSection, out _efCoreSettings);
        _configuration.TryBindOptions(SectionNames.ApplicationSection, out _applicationSettings);
        _services.AddSingleton(_efCoreSettings!);
        _services.AddSingleton(_applicationSettings!);
        _services.AddSingleton(_keycloakProtectionClientOptions!);
    }

    private void ConfigureKeycloak()
    {
        if (_configuration.TryBindOptions(KeycloakProtectionClientOptions.Section,
                out KeycloakProtectionClientOptions options))
        {
            _services.AddSingleton(options);
        }

        _services.AddKeycloakAuthentication(_configuration, jwtOptions =>
        {
            jwtOptions.Events = new JwtBearerEvents
            {
                OnChallenge = async context =>
                {
                    context.HandleResponse();
                    const int statusCode = StatusCodes.Status401Unauthorized;
                    HttpContext httpContext = context.HttpContext;
                    RouteData routeData = httpContext.GetRouteData();
                    ActionContext actionContext = new(httpContext, routeData, new ActionDescriptor());

                    ProblemDetails problemDetails = new()
                    {
                        Status = StatusCodes.Status401Unauthorized,
                        Title = "Unauthorized",
                        Type = "https://tools.ietf.org/html/rfc7235#section-3.1"
                    };

                    ObjectResult result = new(problemDetails)
                    {
                        StatusCode = statusCode
                    };

                    await result.ExecuteResultAsync(actionContext);
                },
                OnForbidden = async context =>
                {
                    const int statusCode = StatusCodes.Status403Forbidden;
                    HttpContext httpContext = context.HttpContext;
                    RouteData routeData = httpContext.GetRouteData();
                    ActionContext actionContext = new(httpContext, routeData, new ActionDescriptor());

                    ProblemDetails problemDetails = new()
                    {
                        Status = StatusCodes.Status403Forbidden,
                        Title = "Forbidden",
                        Type = "https://tools.ietf.org/html/rfc7231#section-6.5.3"
                    };

                    ObjectResult result = new(problemDetails)
                    {
                        StatusCode = statusCode
                    };

                    await result.ExecuteResultAsync(actionContext);
                }
            };
        });

        _services.AddKeycloakAuthorization(_configuration);
        _services.AddKeycloakAdminHttpClient(_configuration);
    }
}