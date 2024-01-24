using Expenso.BudgetSharing.Core.Application.DTO.AssignParticipant;
using Expenso.Shared.Commands;

namespace Expenso.BudgetSharing.Core.Application.Commands.AssignParticipant;

public sealed record AssignParticipantCommand(AssignParticipantRequest AssignParticipant) : ICommand;