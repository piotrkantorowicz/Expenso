using Expenso.BudgetSharing.Domain.BudgetPermissionRequests;
using Expenso.BudgetSharing.Domain.BudgetPermissionRequests.Repositories;
using Expenso.BudgetSharing.Domain.BudgetPermissionRequests.ValueObjects;
using Expenso.BudgetSharing.Domain.BudgetPermissions.Repositories;
using Expenso.Shared.Commands;
using Expenso.Shared.Types.Exceptions;

namespace Expenso.BudgetSharing.Application.Write.ConfirmAssigningParticipat;

internal sealed class ConfirmAssigningParticipantCommandHandler(
    IBudgetPermissionRepository budgetPermissionRepository,
    IBudgetPermissionRequestRepository budgetPermissionRequestRepository)
    : ICommandHandler<ConfirmAssigningParticipantCommand>
{
    private readonly IBudgetPermissionRepository _budgetPermissionRepository = budgetPermissionRepository ??
                                                                               throw new ArgumentNullException(
                                                                                   nameof(budgetPermissionRepository));

    private readonly IBudgetPermissionRequestRepository _budgetPermissionRequestRepository =
        budgetPermissionRequestRepository ?? throw new ArgumentNullException(nameof(budgetPermissionRequestRepository));

    public async Task HandleAsync(ConfirmAssigningParticipantCommand command,
        CancellationToken cancellationToken = default)
    {
        BudgetPermissionRequest? permissionRequest =
            await _budgetPermissionRequestRepository.GetByIdAsync(
                BudgetPermissionRequestId.New(command.BudgetPermissionRequestId), cancellationToken);

        if (permissionRequest is null)
        {
            throw new NotFoundException(
                $"Budget permission request with id {command.BudgetPermissionRequestId} hasn't been found");
        }

        permissionRequest.Confirm();
        await _budgetPermissionRequestRepository.UpdateAsync(permissionRequest, cancellationToken);
    }
}