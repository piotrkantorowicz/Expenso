using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Expenso.Shared.Database.EfCore.NpSql.Migrations;

internal sealed class DbMigrator : IDbMigrator
{
    public void EnsureDatabaseCreated(IServiceScope scope)
    {
        List<Type> dbContexts = AppDomain
            .CurrentDomain.GetAssemblies()
            .SelectMany(a => a.GetTypes())
            .Where(type => typeof(DbContext).IsAssignableFrom(type))
            .Where(type => !type.IsAbstract)
            .Where(type => type != typeof(DbContext))
            .ToList();

        IEnumerable<Type> dbContextThatShouldMigrate =
            dbContexts.Where(db => typeof(IDoNotMigrate).IsAssignableFrom(db) is false);

        foreach (Type context in dbContextThatShouldMigrate)
        {
            (scope.ServiceProvider.GetService(context) as DbContext)?.Database.Migrate();
        }
    }
}