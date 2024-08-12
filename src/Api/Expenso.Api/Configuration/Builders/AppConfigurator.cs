using System.Reflection;

using Expenso.Api.Configuration.Builders.Interfaces;
using Expenso.Api.Configuration.Execution.Middlewares;
using Expenso.Api.Configuration.Extensions.Environment;
using Expenso.BudgetSharing.Domain.Shared;
using Expenso.Shared.Database.EfCore;
using Expenso.Shared.Database.EfCore.Migrations;
using Expenso.Shared.System.Modules;
using Expenso.Shared.System.Tasks;
using Expenso.Shared.System.Types.ExecutionContext;
using Expenso.Shared.System.Types.ExecutionContext.Models;

namespace Expenso.Api.Configuration.Builders;

internal sealed class AppConfigurator(WebApplication app) : IAppConfigurator
{
    private const string BaseTag = "Expenso";

    public IAppConfigurator UseSwagger()
    {
        if (!(app.Environment.IsDevelopment() || app.Environment.IsTest()))
        {
            return this;
        }

        app.UseSwagger();
        app.UseSwaggerUI();

        return this;
    }

    public IAppConfigurator UseAuth()
    {
        app.UseAuthentication();
        app.UseAuthorization();

        return this;
    }

    public IAppConfigurator UseHttpsRedirection()
    {
        app.UseHttpsRedirection();

        return this;
    }

    public IAppConfigurator UseRequestsCorrelation()
    {
        app.UseMiddleware<CorrelationIdMiddleware>();

        return this;
    }

    public IAppConfigurator UseErrorHandler()
    {
        app.UseExceptionHandler();

        return this;
    }

    public IAppConfigurator UseResolvers()
    {
        MessageContextFactoryResolver.BindResolver(serviceProvider: app.Services);

        return this;
    }

    public IAppConfigurator CreateEndpoints()
    {
        app.MapModulesEndpoints(rootTag: BaseTag);

        app
            .MapGet(pattern: "/greetings/hello", handler: (HttpContext httpContext) =>
            {
                httpContext.Response.WriteAsJsonAsync(value: "Hello, I'm Expenso API");
            })
            .WithOpenApi()
            .WithName(endpointName: "Hello")
            .WithTags(BaseTag);

        app
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
        using (IServiceScope scope = app.Services.CreateScope())
        {
            EfCoreSettings? efCoreSettings = scope.ServiceProvider.GetService<EfCoreSettings>();

            if (efCoreSettings is null)
            {
                throw new InvalidOperationException(
                    message: "EfCoreSettings is not registered in the service collection");
            }

            IReadOnlyCollection<Assembly> assemblies = Modules.GetRequiredModulesAssemblies();
            IDbMigrator dbMigrator = scope.ServiceProvider.GetService<IDbMigrator>()!;

            if (efCoreSettings.InMemory is not true)
            {
                dbMigrator.MigrateAsync(scope: scope, assemblies: assemblies, cancellationToken: default).RunAsSync();
            }

            dbMigrator.SeedAsync(scope: scope, assemblies: assemblies, cancellationToken: default).RunAsSync();
        }

        return this;
    }

    public void Run()
    {
        app.Run();
    }
}