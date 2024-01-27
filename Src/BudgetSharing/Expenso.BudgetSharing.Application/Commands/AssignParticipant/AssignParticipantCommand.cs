using Expenso.BudgetSharing.Application.DTO.AssignParticipant;
using Expenso.Shared.Commands;

namespace Expenso.BudgetSharing.Application.Commands.AssignParticipant;

public sealed record AssignParticipantCommand(AssignParticipantRequest AssignParticipant) : ICommand;