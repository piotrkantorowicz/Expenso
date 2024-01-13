using Microsoft.Extensions.DependencyInjection;

namespace Expenso.Shared.Database.EfCore.NpSql.Migrations;

public interface IDbMigrator
{
    Task EnsureDatabaseCreatedAsync(IServiceScope scope);
}