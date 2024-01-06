using System.Text;

using Expenso.Api.Configuration.Builders.Interfaces;
using Expenso.Api.Configuration.Extensions.Environment;
using Expenso.Shared.Database.EfCore;
using Expenso.Shared.Database.EfCore.NpSql.Migrations;
using Expenso.Shared.ModuleDefinition;
using Expenso.Shared.UserContext;

namespace Expenso.Api.Configuration.Builders;

internal sealed class AppConfigurator(WebApplication app) : IAppConfigurator
{
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
        app.MapModulesEndpoints();

        app
            .MapGet("/greetings/hello", (HttpContext httpContext) =>
            {
                httpContext.Response.WriteAsJsonAsync("Hello, I'm Expenso API.");
            })
            .WithName("Hello")
            .WithOpenApi();

        app
            .MapGet("/greetings/hello-user", (HttpContext httpContext) =>
            {
                IUserContextAccessor userContextAccessor =
                    (IUserContextAccessor)httpContext.RequestServices.GetService(typeof(IUserContextAccessor))!;

                IUserContext? userContext = userContextAccessor.Get();

                httpContext.Response.WriteAsJsonAsync(new StringBuilder()
                    .Append("Hello ")
                    .Append(userContext?.Username)
                    .Append(", I'm Expenso API.")
                    .ToString());
            })
            .WithName("HelloUser")
            .WithOpenApi()
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
            
            IDbMigrator dbMigrator = scope.ServiceProvider.GetService<IDbMigrator>()!;
            dbMigrator.EnsureDatabaseCreated(scope);
        }

        return this;
    }

    public void Run()
    {
        app.Run();
    }
}