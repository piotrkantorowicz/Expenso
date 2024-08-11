using Expenso.Shared.Commands;
using Expenso.Shared.System.Types.Messages.Interfaces;

namespace Expenso.BudgetSharing.Application.BudgetPermissionRequests.Write.CancelAssigningParticipant;

public sealed record CancelAssigningParticipantCommand(IMessageContext MessageContext, Guid BudgetPermissionRequestId)
    : ICommand;