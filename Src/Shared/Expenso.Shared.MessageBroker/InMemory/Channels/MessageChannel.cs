using System.Threading.Channels;

using Expenso.Shared.IntegrationEvents;

namespace Expenso.Shared.MessageBroker.InMemory.Channels;

internal sealed class MessageChannel : IMessageChannel
{
    private readonly Channel<IIntegrationEvent> _messages = Channel.CreateUnbounded<IIntegrationEvent>();

    public ChannelReader<IIntegrationEvent> Reader => _messages.Reader;

    public ChannelWriter<IIntegrationEvent> Writer => _messages.Writer;
}