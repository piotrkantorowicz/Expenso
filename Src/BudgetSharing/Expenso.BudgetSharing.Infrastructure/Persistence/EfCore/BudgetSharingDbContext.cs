using Expenso.BudgetSharing.Domain.BudgetPermissionRequests;
using Expenso.BudgetSharing.Domain.BudgetPermissions;
using Expenso.Shared.Domain.Types.Aggregates;
using Expenso.Shared.Domain.Types.Events;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Expenso.BudgetSharing.Infrastructure.Persistence.EfCore;

internal sealed class BudgetSharingDbContext(DbContextOptions<BudgetSharingDbContext> options)
    : DbContext(options), IBudgetSharingDbContext
{
    private IDbContextTransaction? _currentTransaction;

    public DbSet<BudgetPermission> BudgetPermissions { get; set; } = null!;

    public DbSet<BudgetPermissionRequest> BudgetPermissionRequests { get; set; } = null!;

    public EntityState GetEntryState(object entity)
    {
        return Entry(entity).State;
    }

    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        _currentTransaction = await Database.BeginTransactionAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<IDomainEvent>> CommitTransactionAsync(
        CancellationToken cancellationToken = default)
    {
        if (_currentTransaction is null)
        {
            throw new InvalidOperationException("Transaction has not been started.");
        }

        try
        {
            List<IDomainEvent> domainEvents = ChangeTracker
                .Entries<IAggregateRoot>()
                .SelectMany(x => x.Entity.GetUncommittedChanges())
                .ToList();

            await _currentTransaction.CommitAsync(cancellationToken);
            DisposeTransaction();

            return domainEvents;
        }
        catch
        {
            await _currentTransaction.RollbackAsync(cancellationToken);
            DisposeTransaction();

            throw;
        }
    }

    public async Task MigrateAsync()
    {
        await Database.MigrateAsync();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("BudgetSharing");
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }

    private void DisposeTransaction()
    {
        _currentTransaction?.Dispose();
        _currentTransaction = null!;
    }
}