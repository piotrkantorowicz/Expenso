using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Expenso.Shared.Database.EfCore.NpSql;

public static class Extensions
{
    public static void AddPostgres<T>(this IServiceCollection services, EfCoreSettings databaseSettings)
        where T : DbContext
    {
        services.AddDbContext<T>(optionsAction: x => x.UseNpgsql(connectionString: databaseSettings.ConnectionString));
    }
}