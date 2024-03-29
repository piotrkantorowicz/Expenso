using System.Reflection;

using Expenso.Shared.Database.EfCore.DbContexts;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Expenso.Shared.Database.EfCore.Migrations;

internal sealed class DbMigrator : IDbMigrator
{
    public async Task MigrateAsync(IServiceScope scope, IEnumerable<Assembly> assemblies, CancellationToken cancellationToken)
    {
        IEnumerable<Type> dbContextThatShouldMigrate =
            GetDbContexts(assemblies).Where(db => typeof(IDoNotMigrate).IsAssignableFrom(db) is false);

        foreach (Type context in dbContextThatShouldMigrate)
        {
            if (scope.ServiceProvider.GetService(context) is IDbContext dbContext)
            {
                await dbContext.MigrateAsync(cancellationToken);
            }
        }
    }

    public async Task SeedAsync(IServiceScope scope, IEnumerable<Assembly> assemblies, CancellationToken cancellationToken)
    {
        IEnumerable<Type> dbContextThatShouldSeed =
            GetDbContexts(assemblies).Where(db => typeof(IDoNotSeed).IsAssignableFrom(db) is false);

        foreach (Type context in dbContextThatShouldSeed)
        {
            if (scope.ServiceProvider.GetService(context) is IDbContext dbContext)
            {
                await dbContext.SeedAsync(cancellationToken);
            }
        }
    }
    
    private static IEnumerable<Type> GetDbContexts(IEnumerable<Assembly> assemblies)
    {
        return assemblies
            .SelectMany(a => a.GetTypes())
            .Where(type => typeof(DbContext).IsAssignableFrom(type))
            .Where(type => !type.IsAbstract)
            .Where(type => type != typeof(DbContext));
    }
}