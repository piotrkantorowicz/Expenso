using Expenso.BudgetSharing.Domain.BudgetPermissionRequests;
using Expenso.BudgetSharing.Domain.BudgetPermissions;
using Expenso.Shared.Domain.Types.Aggregates;
using Expenso.Shared.Domain.Types.Events;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Expenso.BudgetSharing.Infrastructure.Persistence.EfCore;

internal class BudgetSharingDbContext : DbContext, IBudgetSharingDbContext
{
    private IDbContextTransaction? _currentTransaction;

    public BudgetSharingDbContext(DbContextOptions<BudgetSharingDbContext> options) : base(options: options)
    {
    }

    public DbSet<BudgetPermission> BudgetPermissions { get; set; } = null!;

    public DbSet<BudgetPermissionRequest> BudgetPermissionRequests { get; set; } = null!;

    public EntityState GetEntryState(object entity)
    {
        return Entry(entity: entity).State;
    }

    public async Task BeginTransactionAsync(CancellationToken cancellationToken)
    {
        _currentTransaction = await Database.BeginTransactionAsync(cancellationToken: cancellationToken);
    }

    public async Task RollbackTransactionAsync(CancellationToken cancellationToken)
    {
        if (_currentTransaction is null)
        {
            throw new InvalidOperationException(message: "Transaction has not been started");
        }

        await _currentTransaction.RollbackAsync(cancellationToken: cancellationToken);
        DisposeTransaction();
    }

    public async Task CommitTransactionAsync(CancellationToken cancellationToken)
    {
        if (_currentTransaction is null)
        {
            throw new InvalidOperationException(message: "Transaction has not been started");
        }

        await _currentTransaction.CommitAsync(cancellationToken: cancellationToken);
        DisposeTransaction();
    }

    public IReadOnlyCollection<IDomainEvent> GetUncommittedChanges()
    {
        List<IDomainEvent> domainEvents = ChangeTracker
            .Entries<IAggregateRoot>()
            .SelectMany(selector: x => x.Entity.GetUncommittedChanges())
            .ToList();

        return domainEvents.AsReadOnly();
    }

    public async Task MigrateAsync(CancellationToken cancellationToken)
    {
        await Database.MigrateAsync(cancellationToken: cancellationToken);
    }

    public Task SeedAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(schema: "BudgetSharing");
        modelBuilder.ApplyConfigurationsFromAssembly(assembly: GetType().Assembly);
    }

    private void DisposeTransaction()
    {
        _currentTransaction?.Dispose();
        _currentTransaction = null!;
    }
}