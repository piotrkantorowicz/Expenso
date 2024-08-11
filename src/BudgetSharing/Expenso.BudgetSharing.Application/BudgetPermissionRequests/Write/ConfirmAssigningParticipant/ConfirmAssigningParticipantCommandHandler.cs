using Expenso.BudgetSharing.Domain.BudgetPermissionRequests.Services.Interfaces;
using Expenso.Shared.Commands;

namespace Expenso.BudgetSharing.Application.BudgetPermissionRequests.Write.ConfirmAssigningParticipant;

internal sealed class ConfirmAssigningParticipantCommandHandler(
    IConfirmParticipationDomainService confirmParticipationDomainService)
    : ICommandHandler<ConfirmAssigningParticipantCommand>
{
    private readonly IConfirmParticipationDomainService _confirmParticipationDomainService =
        confirmParticipationDomainService ??
        throw new ArgumentNullException(paramName: nameof(confirmParticipationDomainService));

    public async Task HandleAsync(ConfirmAssigningParticipantCommand command, CancellationToken cancellationToken)
    {
        await _confirmParticipationDomainService.ConfirmParticipationAsync(
            budgetPermissionRequestId: command.BudgetPermissionRequestId, cancellationToken: cancellationToken);
    }
}