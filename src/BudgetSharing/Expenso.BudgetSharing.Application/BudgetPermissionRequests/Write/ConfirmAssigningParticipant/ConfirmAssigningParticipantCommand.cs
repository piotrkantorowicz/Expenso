using Expenso.BudgetSharing.Application.BudgetPermissionRequests.Write.ConfirmAssigningParticipant.DTO;
using Expenso.Shared.Commands;
using Expenso.Shared.System.Types.Messages.Interfaces;

namespace Expenso.BudgetSharing.Application.BudgetPermissionRequests.Write.ConfirmAssigningParticipant;

public sealed record ConfirmAssigningParticipantCommand(
    IMessageContext MessageContext,
    ConfirmAssigningParticipantRequest? Payload) : ICommand;