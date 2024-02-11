using System.Reflection;

using Expenso.Shared.Database.EfCore.NpSql.DbContexts;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Expenso.Shared.Database.EfCore.NpSql.Migrations;

internal sealed class DbMigrator : IDbMigrator
{
    public async Task EnsureDatabaseCreatedAsync(IServiceScope scope, IEnumerable<Assembly> assemblies)
    {
        List<Type> dbContexts = assemblies
            .SelectMany(a => a.GetTypes())
            .Where(type => typeof(DbContext).IsAssignableFrom(type))
            .Where(type => !type.IsAbstract)
            .Where(type => type != typeof(DbContext))
            .ToList();

        IEnumerable<Type> dbContextThatShouldMigrate =
            dbContexts.Where(db => typeof(IDoNotMigrate).IsAssignableFrom(db) is false);

        foreach (Type context in dbContextThatShouldMigrate)
        {
            if (scope.ServiceProvider.GetService(context) is IDbContext dbContext)
            {
                await dbContext.MigrateAsync();
            }
        }
    }
}