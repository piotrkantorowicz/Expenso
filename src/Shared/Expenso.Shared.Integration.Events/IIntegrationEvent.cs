using Expenso.Shared.System.Types.Messages.Interfaces;

namespace Expenso.Shared.Integration.Events;

public interface IIntegrationEvent
{
    IMessageContext MessageContext { get; }
}