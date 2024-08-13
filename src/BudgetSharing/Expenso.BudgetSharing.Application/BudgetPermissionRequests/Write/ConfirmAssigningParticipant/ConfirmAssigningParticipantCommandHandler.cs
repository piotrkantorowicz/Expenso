using Expenso.BudgetSharing.Domain.BudgetPermissionRequests.Services.Interfaces;
using Expenso.Shared.Commands;

namespace Expenso.BudgetSharing.Application.BudgetPermissionRequests.Write.ConfirmAssigningParticipant;

internal sealed class ConfirmAssigningParticipantCommandHandler(
    IConfirmParticipantDomainService confirmParticipantDomainService)
    : ICommandHandler<ConfirmAssigningParticipantCommand>
{
    private readonly IConfirmParticipantDomainService _confirmParticipantDomainService =
        confirmParticipantDomainService ??
        throw new ArgumentNullException(paramName: nameof(confirmParticipantDomainService));

    public async Task HandleAsync(ConfirmAssigningParticipantCommand command, CancellationToken cancellationToken)
    {
        await _confirmParticipantDomainService.ConfirmParticipantAsync(
            budgetPermissionRequestId: command.BudgetPermissionRequestId, cancellationToken: cancellationToken);
    }
}