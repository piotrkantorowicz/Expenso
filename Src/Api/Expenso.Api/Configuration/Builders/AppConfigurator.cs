using System.Reflection;
using System.Text;

using Expenso.Api.Configuration.Builders.Interfaces;
using Expenso.Api.Configuration.Extensions.Environment;
using Expenso.Shared.Database.EfCore;
using Expenso.Shared.Database.EfCore.NpSql.Migrations;
using Expenso.Shared.System.Modules;
using Expenso.Shared.System.Types.UserContext;

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

    public IAppConfigurator UseErrorHandler()
    {
        app.UseExceptionHandler();

        return this;
    }

    public IAppConfigurator CreateEndpoints()
    {
        app.MapModulesEndpoints(BaseTag);

        app
            .MapGet("/greetings/hello", (HttpContext httpContext) =>
            {
                httpContext.Response.WriteAsJsonAsync("Hello, I'm Expenso API.");
            })
            .WithOpenApi()
            .WithName("Hello")
            .WithTags(BaseTag);

        app
            .MapGet("/greetings/hello-user", (HttpContext httpContext) =>
            {
                IUserContextAccessor userContextAccessor =
                    (IUserContextAccessor)httpContext.RequestServices.GetService(typeof(IUserContextAccessor))!;

                IUserContext? userContext = userContextAccessor.Get();

                string response = new StringBuilder()
                    .Append("Hello ")
                    .Append(userContext?.Username)
                    .Append(", I'm Expenso API.")
                    .ToString();

                httpContext.Response.WriteAsJsonAsync(response);
            })
            .WithOpenApi()
            .WithName("HelloUser")
            .WithTags(BaseTag)
            .RequireAuthorization();

        return this;
    }

    public IAppConfigurator MigrateDatabase()
    {
        using (IServiceScope scope = app.Services.CreateScope())
        {
            EfCoreSettings? efCoreSettings = scope.ServiceProvider.GetService<EfCoreSettings>();

            if (efCoreSettings?.InMemory == true)
            {
                return this;
            }

            IReadOnlyCollection<Assembly> assemblies = Modules.GetRequiredModulesAssemblies();
            IDbMigrator dbMigrator = scope.ServiceProvider.GetService<IDbMigrator>()!;
            Task runMigrationsTask = dbMigrator.EnsureDatabaseCreatedAsync(scope, assemblies);
            Task.Run(() => runMigrationsTask).Wait();
        }

        return this;
    }

    public void Run()
    {
        app.Run();
    }
}