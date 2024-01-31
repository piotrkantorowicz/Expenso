using Expenso.Shared.Commands;

namespace Expenso.BudgetSharing.Application.Commands.CancelAssigningParticipant;

public sealed record CancelAssigningParticipantCommand(Guid BudgetPermissionRequestId) : ICommand;