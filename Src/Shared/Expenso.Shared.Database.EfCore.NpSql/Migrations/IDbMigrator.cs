using Microsoft.Extensions.DependencyInjection;

namespace Expenso.Shared.Database.EfCore.NpSql.Migrations;

public interface IDbMigrator
{
    void EnsureDatabaseCreated(IServiceScope scope);
}