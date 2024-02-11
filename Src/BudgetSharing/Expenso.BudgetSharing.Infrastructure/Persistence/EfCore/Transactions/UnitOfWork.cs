using Expenso.Shared.Database;
using Expenso.Shared.Domain.Events.Dispatchers;
using Expenso.Shared.Domain.Types.Events;

namespace Expenso.BudgetSharing.Infrastructure.Persistence.EfCore.Transactions;

internal sealed class UnitOfWork(IBudgetSharingDbContext budgetSharingDbContext, IDomainEventBroker domainEventBroker)
    : IUnitOfWork
{
    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        await budgetSharingDbContext.BeginTransactionAsync(cancellationToken);
    }

    public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
    {
        await budgetSharingDbContext.RollbackTransactionAsync(cancellationToken);
    }

    public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        IReadOnlyCollection<IDomainEvent> domainEvents = budgetSharingDbContext.GetUncommittedChanges();
        await budgetSharingDbContext.CommitTransactionAsync(cancellationToken);
        await domainEventBroker.PublishMultipleAsync(domainEvents, cancellationToken);
    }
}