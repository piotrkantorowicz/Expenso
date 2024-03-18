using Expenso.Shared.Commands;
using Expenso.Shared.System.Types.Messages.Interfaces;

namespace Expenso.TimeManagement.Core.Application.Jobs.Write.RegisterJob;

public sealed record RegisterJobCommand(IMessageContext MessageContext) : ICommand
{
}