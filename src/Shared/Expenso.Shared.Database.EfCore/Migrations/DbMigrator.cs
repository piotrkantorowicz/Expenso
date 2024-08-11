using System.Reflection;

using Expenso.Shared.Database.EfCore.DbContexts;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Expenso.Shared.Database.EfCore.Migrations;

internal sealed class DbMigrator : IDbMigrator
{
    public async Task MigrateAsync(IServiceScope scope, IEnumerable<Assembly> assemblies,
        CancellationToken cancellationToken)
    {
        IEnumerable<Type> dbContextThatShouldMigrate = GetDbContexts(assemblies: assemblies)
            .Where(predicate: db => typeof(IDoNotMigrate).IsAssignableFrom(c: db) is false);

        foreach (Type context in dbContextThatShouldMigrate)
        {
            if (scope.ServiceProvider.GetService(serviceType: context) is IDbContext dbContext)
            {
                await dbContext.MigrateAsync(cancellationToken: cancellationToken);
            }
        }
    }

    public async Task SeedAsync(IServiceScope scope, IEnumerable<Assembly> assemblies,
        CancellationToken cancellationToken)
    {
        IEnumerable<Type> dbContextThatShouldSeed = GetDbContexts(assemblies: assemblies)
            .Where(predicate: db => typeof(IDoNotSeed).IsAssignableFrom(c: db) is false);

        foreach (Type context in dbContextThatShouldSeed)
        {
            if (scope.ServiceProvider.GetService(serviceType: context) is IDbContext dbContext)
            {
                await dbContext.SeedAsync(cancellationToken: cancellationToken);
            }
        }
    }

    private static IEnumerable<Type> GetDbContexts(IEnumerable<Assembly> assemblies)
    {
        return assemblies
            .SelectMany(selector: a => a.GetTypes())
            .Where(predicate: type => typeof(DbContext).IsAssignableFrom(c: type))
            .Where(predicate: type => !type.IsAbstract)
            .Where(predicate: type => type != typeof(DbContext));
    }
}