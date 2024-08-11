using Expenso.Shared.Database.EfCore.Migrations;

using Microsoft.Extensions.DependencyInjection;

namespace Expenso.Shared.Database.EfCore;

public static class Extensions
{
    public static void AddDbMigrator(this IServiceCollection services)
    {
        services.AddScoped<IDbMigrator, DbMigrator>();
    }
}