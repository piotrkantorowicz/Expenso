using Expenso.Shared.Commands;

namespace Expenso.BudgetSharing.Application.Commands.ConfirmParticipation;

public sealed record ConfirmParticipationCommand(Guid BudgetPermissionRequestId, Guid BudgetPermissionId) : ICommand;
