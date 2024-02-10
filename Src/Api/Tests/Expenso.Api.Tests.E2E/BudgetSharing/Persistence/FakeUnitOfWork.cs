using Expenso.BudgetSharing.Infrastructure.Persistence.EfCore;
using Expenso.Shared.Database;
using Expenso.Shared.Domain.Events.Dispatchers;
using Expenso.Shared.Domain.Types.Events;

namespace Expenso.Api.Tests.E2E.BudgetSharing.Persistence;

// This is a mock for e2e tests with in memory database which not supports transaction scope
internal sealed class FakeUnitOfWork(
    IBudgetSharingDbContext budgetSharingDbContext,
    IDomainEventBroker domainEventBroker) : IUnitOfWork
{
    private readonly IBudgetSharingDbContext _budgetSharingDbContext =
        budgetSharingDbContext ?? throw new ArgumentNullException(nameof(budgetSharingDbContext));

    private readonly IDomainEventBroker _domainEventBroker =
        domainEventBroker ?? throw new ArgumentNullException(nameof(domainEventBroker));

    public Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }

    public Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }

    public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        IReadOnlyCollection<IDomainEvent> domainEvents = _budgetSharingDbContext.GetUncommittedChanges();
        await _domainEventBroker.PublishMultipleAsync(domainEvents, cancellationToken);
    }
}