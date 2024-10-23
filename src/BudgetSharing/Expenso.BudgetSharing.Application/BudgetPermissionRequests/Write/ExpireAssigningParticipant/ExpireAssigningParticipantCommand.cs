using Expenso.BudgetSharing.Application.BudgetPermissionRequests.Write.ExpireAssigningParticipant.DTO;
using Expenso.Shared.Commands;
using Expenso.Shared.System.Types.Messages.Interfaces;

namespace Expenso.BudgetSharing.Application.BudgetPermissionRequests.Write.ExpireAssigningParticipant;

public sealed record ExpireAssigningParticipantCommand(
    IMessageContext MessageContext,
    ExpireAssigningParticipantRequest? Payload) : ICommand;