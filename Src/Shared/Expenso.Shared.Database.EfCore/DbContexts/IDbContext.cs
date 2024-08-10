namespace Expenso.Shared.Database.EfCore.DbContexts;

public interface IDbContext
{
    Task MigrateAsync(CancellationToken cancellationToken);

    Task SeedAsync(CancellationToken cancellationToken);

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}