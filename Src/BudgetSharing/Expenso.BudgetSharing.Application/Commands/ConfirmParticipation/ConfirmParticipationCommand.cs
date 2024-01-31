using Expenso.BudgetSharing.Application.DTO.ConfirmParticipation.Requests;
using Expenso.Shared.Commands;

namespace Expenso.BudgetSharing.Application.Commands.ConfirmParticipation;

public sealed record ConfirmParticipationCommand(
    Guid BudgetPermissionRequestId,
    ConfirmParticipationRequest ConfirmParticipationRequest) : ICommand;