using Expenso.BudgetSharing.Application.Write.AssignParticipant.DTO.Requests;
using Expenso.Shared.Commands;

namespace Expenso.BudgetSharing.Application.Write.AssignParticipant;

public sealed record AssignParticipantCommand(AssignParticipantRequest AssignParticipantRequest) : ICommand;