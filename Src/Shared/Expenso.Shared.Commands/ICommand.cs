using Expenso.Shared.System.Types.Messages.Interfaces;

namespace Expenso.Shared.Commands;

public interface ICommand
{
    IMessageContext MessageContext { get; }
}