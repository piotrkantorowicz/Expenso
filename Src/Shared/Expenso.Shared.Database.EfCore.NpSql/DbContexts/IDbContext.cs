namespace Expenso.Shared.Database.EfCore.NpSql.DbContexts;

public interface IDbContext
{
    Task MigrateAsync();
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}