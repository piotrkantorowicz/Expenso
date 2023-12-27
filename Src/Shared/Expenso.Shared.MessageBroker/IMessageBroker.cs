using Expenso.Shared.IntegrationEvents;

namespace Expenso.Shared.MessageBroker;

public interface IMessageBroker
{
    Task PublishAsync<TIntegrationEvent>(TIntegrationEvent message, CancellationToken cancellationToken = default)
        where TIntegrationEvent : IIntegrationEvent;
}