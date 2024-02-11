using Expenso.Shared.Commands;

namespace Expenso.BudgetSharing.Application.Write.ConfirmAssigningParticipat;

public sealed record ConfirmAssigningParticipantCommand(Guid BudgetPermissionRequestId) : ICommand;