using System.Threading.Channels;

using Expenso.Shared.Integration.Events;

namespace Expenso.Shared.Integration.MessageBroker.InMemory.Channels;

internal interface IMessageChannel
{
    ChannelReader<IIntegrationEvent> Reader { get; }

    ChannelWriter<IIntegrationEvent> Writer { get; }
}