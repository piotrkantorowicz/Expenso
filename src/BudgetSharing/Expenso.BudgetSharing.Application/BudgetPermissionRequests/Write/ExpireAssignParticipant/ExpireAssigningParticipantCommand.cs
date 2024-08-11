using Expenso.Shared.Commands;
using Expenso.Shared.System.Types.Messages.Interfaces;

namespace Expenso.BudgetSharing.Application.BudgetPermissionRequests.Write.ExpireAssignParticipant;

public sealed record ExpireAssigningParticipantCommand(IMessageContext MessageContext, Guid BudgetPermissionRequestId)
    : ICommand;