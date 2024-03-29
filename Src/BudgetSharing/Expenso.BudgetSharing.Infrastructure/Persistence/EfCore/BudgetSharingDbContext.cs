using Expenso.BudgetSharing.Domain.BudgetPermissionRequests;
using Expenso.BudgetSharing.Domain.BudgetPermissions;
using Expenso.Shared.Domain.Types.Aggregates;
using Expenso.Shared.Domain.Types.Events;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Expenso.BudgetSharing.Infrastructure.Persistence.EfCore;

internal class BudgetSharingDbContext(DbContextOptions<BudgetSharingDbContext> options)
    : DbContext(options), IBudgetSharingDbContext
{
    private IDbContextTransaction? _currentTransaction;

    public DbSet<BudgetPermission> BudgetPermissions { get; set; } = null!;

    public DbSet<BudgetPermissionRequest> BudgetPermissionRequests { get; set; } = null!;

    public EntityState GetEntryState(object entity)
    {
        return Entry(entity).State;
    }

    public async Task BeginTransactionAsync(CancellationToken cancellationToken)
    {
        _currentTransaction = await Database.BeginTransactionAsync(cancellationToken);
    }

    public async Task RollbackTransactionAsync(CancellationToken cancellationToken)
    {
        if (_currentTransaction is null)
        {
            throw new InvalidOperationException("Transaction has not been started.");
        }

        await _currentTransaction.RollbackAsync(cancellationToken);
        DisposeTransaction();
    }

    public async Task CommitTransactionAsync(CancellationToken cancellationToken)
    {
        if (_currentTransaction is null)
        {
            throw new InvalidOperationException("Transaction has not been started.");
        }

        await _currentTransaction.CommitAsync(cancellationToken);
        DisposeTransaction();
    }

    public IReadOnlyCollection<IDomainEvent> GetUncommittedChanges()
    {
        List<IDomainEvent> domainEvents = ChangeTracker
            .Entries<IAggregateRoot>()
            .SelectMany(x => x.Entity.GetUncommittedChanges())
            .ToList();

        return domainEvents.AsReadOnly();
    }

    public async Task MigrateAsync(CancellationToken cancellationToken)
    {
        await Database.MigrateAsync(cancellationToken);
    }

    public Task SeedAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("BudgetSharing");
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }

    private void DisposeTransaction()
    {
        // _currentTransaction?.Dispose();
        _currentTransaction = null!;
    }
}