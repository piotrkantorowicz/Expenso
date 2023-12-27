using Expenso.Shared.IntegrationEvents;

namespace Expenso.Shared.MessageBroker;

public interface IMessageBroker
{
    Task PublishAsync<TIntegrationEvent>(TIntegrationEvent @event, CancellationToken cancellationToken = default)
        where TIntegrationEvent : IIntegrationEvent;
}