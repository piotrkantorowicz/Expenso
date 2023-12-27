using Expenso.Shared.IntegrationEvents;
using Expenso.Shared.MessageBroker.InMemory.Channels;

namespace Expenso.Shared.MessageBroker.InMemory;

internal sealed class InMemoryMessageBroker(IMessageChannel channel) : IMessageBroker
{
    private readonly IMessageChannel _channel = channel ?? throw new ArgumentNullException(nameof(channel));

    public async Task PublishAsync<TIntegrationEvent>(TIntegrationEvent message, CancellationToken cancellationToken = default)
        where TIntegrationEvent : IIntegrationEvent
    {
        await PublishAsync(cancellationToken, message);
    }
    
    private async Task PublishAsync(CancellationToken cancellation = default, params IIntegrationEvent[] messages)
    {
        foreach (var message in messages)
        {
            await _channel.Writer.WriteAsync(message, cancellation);
        }
    }
}