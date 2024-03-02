using Expenso.BudgetSharing.Domain.BudgetPermissionRequests;
using Expenso.BudgetSharing.Domain.BudgetPermissionRequests.Repositories;
using Expenso.BudgetSharing.Domain.BudgetPermissionRequests.ValueObjects;
using Expenso.Shared.Commands;
using Expenso.Shared.System.Types.Exceptions;

namespace Expenso.BudgetSharing.Application.BudgetPermissionRequests.Write.CancelAssigningParticipant;

internal sealed class CancelAssigningParticipantCommandHandler(
    IBudgetPermissionRequestRepository budgetPermissionRequestRepository)
    : ICommandHandler<CancelAssigningParticipantCommand>
{
    private readonly IBudgetPermissionRequestRepository _budgetPermissionRequestRepository =
        budgetPermissionRequestRepository ?? throw new ArgumentNullException(nameof(budgetPermissionRequestRepository));

    public async Task HandleAsync(CancelAssigningParticipantCommand command, CancellationToken cancellationToken)
    {
        BudgetPermissionRequest? budgetPermissionRequest =
            await _budgetPermissionRequestRepository.GetByIdAsync(
                BudgetPermissionRequestId.New(command.BudgetPermissionRequestId), cancellationToken);

        if (budgetPermissionRequest is null)
        {
            throw new NotFoundException(
                $"Budget permission request with id {command.BudgetPermissionRequestId} hasn't been found.");
        }

        budgetPermissionRequest.Cancel();
        await _budgetPermissionRequestRepository.UpdateAsync(budgetPermissionRequest, cancellationToken);
    }
}