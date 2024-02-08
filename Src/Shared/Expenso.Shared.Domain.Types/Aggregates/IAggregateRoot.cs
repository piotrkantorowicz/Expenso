using Expenso.Shared.Domain.Types.Events;

namespace Expenso.Shared.Domain.Types.Aggregates;

public interface IAggregateRoot
{
    IReadOnlyCollection<IDomainEvent> GetUncommittedChanges();
}