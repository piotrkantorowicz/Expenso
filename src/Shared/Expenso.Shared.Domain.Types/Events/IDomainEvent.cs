using Expenso.Shared.System.Types.Messages.Interfaces;

namespace Expenso.Shared.Domain.Types.Events;

public interface IDomainEvent
{
    IMessageContext MessageContext { get; }
}