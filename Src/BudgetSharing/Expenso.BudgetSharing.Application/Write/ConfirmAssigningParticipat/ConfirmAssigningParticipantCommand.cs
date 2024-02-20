using Expenso.Shared.Commands;
using Expenso.Shared.System.Types.Messages.Interfaces;

namespace Expenso.BudgetSharing.Application.Write.ConfirmAssigningParticipat;

public sealed record ConfirmAssigningParticipantCommand(IMessageContext MessageContext, Guid BudgetPermissionRequestId)
    : ICommand;