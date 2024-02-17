using System.Threading.Channels;

using Expenso.Shared.Integration.Events;

namespace Expenso.Shared.Integration.MessageBroker.InMemory.Channels;

internal sealed class MessageChannel : IMessageChannel
{
    private readonly Channel<IIntegrationEvent> _messages = Channel.CreateUnbounded<IIntegrationEvent>();

    public ChannelReader<IIntegrationEvent> Reader => _messages.Reader;

    public ChannelWriter<IIntegrationEvent> Writer => _messages.Writer;
}