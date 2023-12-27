using System.Text.Json.Serialization;

using Expenso.Api.Configuration.Auth.Users.UserContext;
using Expenso.Api.Configuration.Builders.Interfaces;
using Expenso.Api.Configuration.Filters;
using Expenso.Api.Configuration.Options;
using Expenso.IAM.Api;
using Expenso.Shared.MessageBroker;
using Expenso.Shared.ModuleDefinition;
using Expenso.Shared.UserContext;

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

    public AppBuilder(string[] args)
    {
        _applicationBuilder = WebApplication.CreateBuilder(args);
        _configuration = _applicationBuilder.Configuration;
        _services = _applicationBuilder.Services;
    }

    public IAppBuilder ConfigureModules()
    {
        Modules.RegisterModule<IamModule>();
        _services.AddModules(_configuration);

        return this;
    }

    public IAppBuilder ConfigureApiDependencies()
    {
        _services.AddScoped<IUserContextAccessor, UserContextAccessor>();
        _services.AddHttpContextAccessor();

        return this;
    }

    public IAppBuilder ConfigureMvc()
    {
        _services.AddControllers(options =>
        {
            options.Filters.Add<ApiExceptionFilterAttribute>();
        });

        _services.AddEndpointsApiExplorer();

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

    public IAppBuilder ConfigureKeycloak()
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

        _services.AddAuthorization().AddKeycloakAuthorization(_configuration);
        _services.AddKeycloakAdminHttpClient(_configuration);

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

    public IAppBuilder ConfigureMessageBroker()
    {
        _services.AddMessageBroker();

        return this;
    }

    public IAppConfigurator Build()
    {
        WebApplication app = _applicationBuilder.Build();

        return new AppConfigurator(app);
    }
}