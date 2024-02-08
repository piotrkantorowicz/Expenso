using Expenso.Shared.Domain.Types.Events;

namespace Expenso.Shared.Domain.Events;

public interface IDomainEventHandler<in TEvent> where TEvent : class, IDomainEvent
{
    Task HandleAsync(TEvent @event, CancellationToken cancellationToken = default);
}