namespace Expenso.Shared.Integration.Events;

public interface IIntegrationEventHandler<in TIntegrationEvent> where TIntegrationEvent : IIntegrationEvent
{
    Task HandleAsync(TIntegrationEvent @event, CancellationToken cancellationToken = default);
}