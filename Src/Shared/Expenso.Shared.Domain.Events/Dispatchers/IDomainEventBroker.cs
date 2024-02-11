using Expenso.Shared.Domain.Types.Events;

namespace Expenso.Shared.Domain.Events.Dispatchers;

public interface IDomainEventBroker
{
    Task PublishAsync<TEvent>(TEvent @event, CancellationToken cancellationToken = default)
        where TEvent : class, IDomainEvent;

    Task PublishMultipleAsync(IEnumerable<IDomainEvent> events, CancellationToken cancellationToken = default);
}