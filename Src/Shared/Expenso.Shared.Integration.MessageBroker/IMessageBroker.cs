using Expenso.Shared.Integration.Events;

namespace Expenso.Shared.Integration.MessageBroker;

public interface IMessageBroker
{
    Task PublishAsync<TIntegrationEvent>(TIntegrationEvent @event, CancellationToken cancellationToken)
        where TIntegrationEvent : IIntegrationEvent;
}