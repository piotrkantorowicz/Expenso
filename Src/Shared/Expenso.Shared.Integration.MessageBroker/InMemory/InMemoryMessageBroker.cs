using Expenso.Shared.Integration.Events;
using Expenso.Shared.Integration.MessageBroker.InMemory.Channels;

namespace Expenso.Shared.Integration.MessageBroker.InMemory;

internal sealed class InMemoryMessageBroker(IMessageChannel channel) : IMessageBroker
{
    private readonly IMessageChannel _channel = channel ?? throw new ArgumentNullException(paramName: nameof(channel));

    public async Task PublishAsync<TIntegrationEvent>(TIntegrationEvent @event, CancellationToken cancellationToken)
        where TIntegrationEvent : IIntegrationEvent
    {
        await PublishAsync(cancellation: cancellationToken, @event);
    }

    private async Task PublishAsync(CancellationToken cancellation = default, params IIntegrationEvent[] events)
    {
        foreach (IIntegrationEvent @event in events)
        {
            await _channel.Writer.WriteAsync(item: @event, cancellationToken: cancellation);
        }
    }
}