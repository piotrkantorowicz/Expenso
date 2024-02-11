using Expenso.Shared.Commands;

namespace Expenso.BudgetSharing.Application.Write.CancelAssigningParticipant;

public sealed record CancelAssigningParticipantCommand(Guid BudgetPermissionRequestId) : ICommand;