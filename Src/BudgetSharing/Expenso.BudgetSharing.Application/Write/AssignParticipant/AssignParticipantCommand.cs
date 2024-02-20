using Expenso.BudgetSharing.Application.Write.AssignParticipant.DTO.Requests;
using Expenso.Shared.Commands;
using Expenso.Shared.System.Types.Messages.Interfaces;

namespace Expenso.BudgetSharing.Application.Write.AssignParticipant;

public sealed record AssignParticipantCommand(
    IMessageContext MessageContext,
    AssignParticipantRequest AssignParticipantRequest) : ICommand;