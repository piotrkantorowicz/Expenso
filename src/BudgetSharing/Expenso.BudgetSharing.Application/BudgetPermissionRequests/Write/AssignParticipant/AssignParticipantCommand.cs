using Expenso.BudgetSharing.Shared.DTO.API.BudgetPermissionRequests.AssignParticipant.Request;
using Expenso.Shared.Commands;
using Expenso.Shared.System.Types.Messages.Interfaces;

namespace Expenso.BudgetSharing.Application.BudgetPermissionRequests.Write.AssignParticipant;

public sealed record AssignParticipantCommand(IMessageContext MessageContext, AssignParticipantRequest? Payload)
    : ICommand;