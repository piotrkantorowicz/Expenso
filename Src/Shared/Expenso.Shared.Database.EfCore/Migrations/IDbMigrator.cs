using System.Reflection;

using Microsoft.Extensions.DependencyInjection;

namespace Expenso.Shared.Database.EfCore.Migrations;

public interface IDbMigrator
{
    Task MigrateAsync(IServiceScope scope, IEnumerable<Assembly> assemblies, CancellationToken cancellationToken);
    Task SeedAsync(IServiceScope scope, IEnumerable<Assembly> assemblies, CancellationToken cancellationToken);
}