using System.Reflection;

using Expenso.Api.Configuration.Builders.Interfaces;
using Expenso.Api.Configuration.Execution.Middlewares;
using Expenso.Api.Configuration.Extensions.Environment;
using Expenso.BudgetSharing.Domain.Shared;
using Expenso.Shared.Database.EfCore.Migrations;
using Expenso.Shared.Database.EfCore.Settings;
using Expenso.Shared.System.Modules;
using Expenso.Shared.System.Tasks;
using Expenso.Shared.System.Types.ExecutionContext;
using Expenso.Shared.System.Types.ExecutionContext.Models;

using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

using Serilog;

namespace Expenso.Api.Configuration.Builders;

internal sealed class AppConfigurator : IAppConfigurator
{
    private const string BaseTag = "Expenso";
    private readonly WebApplication _app;

    public AppConfigurator(WebApplication app)
    {
        _app = app;
    }

    public IAppConfigurator UseSwagger()
    {
        if (!(_app.Environment.IsDevelopment() || _app.Environment.IsTest()))
        {
            return this;
        }

        _app.UseSwagger();
        _app.UseSwaggerUI();

        return this;
    }

    public IAppConfigurator UseAuth()
    {
        _app.UseAuthentication();
        _app.UseAuthorization();

        return this;
    }

    public IAppConfigurator UseHealthChecks()
    {
        _app.MapHealthChecks(pattern: "/health", options: new HealthCheckOptions
        {
            AllowCachingResponses = false,
            ResultStatusCodes =
            {
                [key: HealthStatus.Healthy] = StatusCodes.Status200OK,
                [key: HealthStatus.Degraded] = StatusCodes.Status200OK,
                [key: HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable
            }
        });

        return this;
    }

    public IAppConfigurator UseHttpsRedirection()
    {
        _app.UseHttpsRedirection();

        return this;
    }

    public IAppConfigurator UseRequestsCorrelation()
    {
        _app.UseMiddleware<CorrelationIdMiddleware>();
        _app.UseMiddleware<ModuleIdMiddleware>();

        return this;
    }

    public IAppConfigurator UseErrorHandler()
    {
        _app.UseExceptionHandler();

        return this;
    }

    public IAppConfigurator UseResolvers()
    {
        MessageContextFactoryResolver.BindResolver(serviceProvider: _app.Services);

        return this;
    }

    public IAppConfigurator CreateEndpoints()
    {
        _app.MapModulesEndpoints(rootTag: BaseTag);

        _app
            .MapGet(pattern: "/greetings/hello", handler: (HttpContext httpContext) =>
            {
                httpContext.Response.WriteAsJsonAsync(value: "Hello, I'm Expenso API");
            })
            .WithOpenApi()
            .WithName(endpointName: "Hello")
            .WithTags(BaseTag);

        _app
            .MapGet(pattern: "/greetings/hello-user", handler: (HttpContext httpContext) =>
            {
                IExecutionContextAccessor executionContextAccessor =
                    (IExecutionContextAccessor)httpContext.RequestServices.GetService(
                        serviceType: typeof(IExecutionContextAccessor))!;

                IUserContext? userContext = executionContextAccessor.Get()?.UserContext;
                string response = $"Hello {userContext?.Username}, I'm Expenso API";
                httpContext.Response.WriteAsJsonAsync(value: response);
            })
            .WithOpenApi()
            .WithName(endpointName: "HelloUser")
            .WithTags(BaseTag)
            .RequireAuthorization();

        return this;
    }

    public IAppConfigurator MigrateDatabase()
    {
        using (IServiceScope scope = _app.Services.CreateScope())
        {
            EfCoreSettings? efCoreSettings = scope.ServiceProvider.GetService<EfCoreSettings>();

            if (efCoreSettings is null)
            {
                throw new InvalidOperationException(
                    message: "EfCoreSettings is not registered in the service collection");
            }

            IReadOnlyCollection<Assembly> assemblies = Modules.GetRequiredModulesAssemblies();
            IDbMigrator dbMigrator = scope.ServiceProvider.GetService<IDbMigrator>()!;

            if (efCoreSettings.InMemory is not true && efCoreSettings.UseMigration is true)
            {
                dbMigrator.MigrateAsync(scope: scope, assemblies: assemblies, cancellationToken: default).RunAsSync();
            }

            if (efCoreSettings.UseSeeding is true)
            {
                dbMigrator.SeedAsync(scope: scope, assemblies: assemblies, cancellationToken: default).RunAsSync();
            }
        }

        return this;
    }

    public IAppConfigurator UseRequestsLogging()
    {
        _app.UseSerilogRequestLogging();

        return this;
    }

    public void Run()
    {
        _app.Run();
    }
}