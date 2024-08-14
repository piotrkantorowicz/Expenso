using Expenso.Shared.Database;
using Expenso.Shared.Domain.Events.Dispatchers;
using Expenso.Shared.Domain.Types.Events;

namespace Expenso.BudgetSharing.Infrastructure.Persistence.EfCore.Transactions;

internal sealed class UnitOfWork : IUnitOfWork
{
    private readonly IBudgetSharingDbContext _budgetSharingDbContext;
    private readonly IDomainEventBroker _domainEventBroker;

    public UnitOfWork(IBudgetSharingDbContext budgetSharingDbContext, IDomainEventBroker domainEventBroker)
    {
        _budgetSharingDbContext = budgetSharingDbContext ??
                                  throw new ArgumentNullException(paramName: nameof(budgetSharingDbContext));

        _domainEventBroker = domainEventBroker ?? throw new ArgumentNullException(paramName: nameof(domainEventBroker));
    }

    public async Task BeginTransactionAsync(CancellationToken cancellationToken)
    {
        await _budgetSharingDbContext.BeginTransactionAsync(cancellationToken: cancellationToken);
    }

    public async Task RollbackTransactionAsync(CancellationToken cancellationToken)
    {
        await _budgetSharingDbContext.RollbackTransactionAsync(cancellationToken: cancellationToken);
    }

    public async Task CommitTransactionAsync(CancellationToken cancellationToken, bool dispatchEvents = true)
    {
        IReadOnlyCollection<IDomainEvent> domainEvents = _budgetSharingDbContext.GetUncommittedChanges();
        await _budgetSharingDbContext.CommitTransactionAsync(cancellationToken: cancellationToken);

        if (dispatchEvents)
        {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            Task.Run(
                function: () =>
                    _domainEventBroker.PublishMultipleAsync(events: domainEvents, cancellationToken: default),
                cancellationToken: default);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        }
    }
}