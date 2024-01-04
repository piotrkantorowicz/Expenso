using System.Threading.Channels;

using Expenso.Shared.IntegrationEvents;

namespace Expenso.Shared.MessageBroker.InMemory.Channels;

internal interface IMessageChannel
{
    ChannelReader<IIntegrationEvent> Reader { get; }

    ChannelWriter<IIntegrationEvent> Writer { get; }
}