using Expenso.Shared.Configuration.Extensions;
using Expenso.Shared.Configuration.Sections;
using Expenso.Shared.Database.EfCore.NpSql.Migrations;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Expenso.Shared.Database.EfCore.NpSql;

public static class RegistrationExtensions
{
    public static void AddPostgres<T>(this IServiceCollection services, IConfiguration configuration, string moduleName)
        where T : DbContext
    {
        configuration.TryBindOptions(SectionNames.EfCoreSection, out EfCoreSettings databaseSettings);

        if (databaseSettings.InMemory == true)
        {
            services.AddDbContext<T>(x => x.UseInMemoryDatabase(moduleName));

            return;
        }

        services.AddDbContext<T>(x => x.UseNpgsql(databaseSettings.ConnectionString));
        services.AddScoped<IDbMigrator, DbMigrator>();
    }
}