using Expenso.BudgetSharing.Infrastructure.Persistence.EfCore;
using Expenso.Shared.Database;
using Expenso.Shared.Domain.Events.Dispatchers;
using Expenso.Shared.Domain.Types.Events;

namespace Expenso.Api.Tests.E2E.BudgetSharing.Persistence;

// This is a mock for e2e tests with in memory database which not supports transaction scope
internal sealed class FakeUnitOfWork : IUnitOfWork
{
    private readonly IBudgetSharingDbContext _budgetSharingDbContext;
    private readonly IDomainEventBroker _domainEventBroker;

    public FakeUnitOfWork(IBudgetSharingDbContext budgetSharingDbContext, IDomainEventBroker domainEventBroker)
    {
        _budgetSharingDbContext = budgetSharingDbContext ??
                                  throw new ArgumentNullException(paramName: nameof(budgetSharingDbContext));

        _domainEventBroker = domainEventBroker ?? throw new ArgumentNullException(paramName: nameof(domainEventBroker));
    }

    public Task BeginTransactionAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public Task RollbackTransactionAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public Task CommitTransactionAsync(CancellationToken cancellationToken, bool dispatchEvents = true)
    {
        if (dispatchEvents)
        {
            IReadOnlyCollection<IDomainEvent> domainEvents = _budgetSharingDbContext.GetUncommittedChanges();
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            Task.Run(
                function: () =>
                    _domainEventBroker.PublishMultipleAsync(events: domainEvents, cancellationToken: default),
                cancellationToken: default);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        }

        return Task.CompletedTask;
    }
}