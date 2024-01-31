using Expenso.BudgetSharing.Application.DTO.AssignParticipant.Requests;
using Expenso.Shared.Commands;

namespace Expenso.BudgetSharing.Application.Commands.AssignParticipant;

public sealed record AssignParticipantCommand(AssignParticipantRequest AssignParticipantRequest) : ICommand;