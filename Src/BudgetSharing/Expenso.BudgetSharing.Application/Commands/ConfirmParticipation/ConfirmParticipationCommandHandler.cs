using Expenso.BudgetSharing.Domain.BudgetPermissionRequests.Model;
using Expenso.BudgetSharing.Domain.BudgetPermissionRequests.Repositories;
using Expenso.BudgetSharing.Domain.BudgetPermissions.Model;
using Expenso.BudgetSharing.Domain.BudgetPermissions.Repositories;
using Expenso.Shared.Commands;
using Expenso.Shared.Types.Exceptions;

namespace Expenso.BudgetSharing.Application.Commands.ConfirmParticipation;

internal sealed class ConfirmParticipationCommandHandler(
    IBudgetPermissionRepository budgetPermissionRepository,
    IBudgetPermissionRequestRepository budgetPermissionRequestRepository) : ICommandHandler<ConfirmParticipationCommand>
{
    private readonly IBudgetPermissionRepository _budgetPermissionRepository = budgetPermissionRepository ??
                                                                               throw new ArgumentNullException(
                                                                                   nameof(budgetPermissionRepository));

    private readonly IBudgetPermissionRequestRepository _budgetPermissionRequestRepository =
        budgetPermissionRequestRepository ?? throw new ArgumentNullException(nameof(budgetPermissionRequestRepository));

    public async Task HandleAsync(ConfirmParticipationCommand command, CancellationToken cancellationToken = default)
    {
        (Guid budgetPermissionRequestId, Guid budgetPermissionId) = command;

        BudgetPermissionRequest? permissionRequest =
            await _budgetPermissionRequestRepository.GetByIdAsync(budgetPermissionRequestId, true, cancellationToken);

        if (permissionRequest is null)
        {
            throw new NotFoundException(
                $"Budget permission request with id {budgetPermissionRequestId} hasn't been found");
        }

        permissionRequest.Confirm();
        await _budgetPermissionRequestRepository.UpdateAsync(permissionRequest, cancellationToken);

        BudgetPermission? budgetPermission =
            await _budgetPermissionRepository.GetByIdAsync(budgetPermissionId, true, true, cancellationToken);

        if (budgetPermission is null)
        {
            throw new NotFoundException($"Budget permission with id {budgetPermissionId} hasn't been found");
        }

        budgetPermission.AddPermission(permissionRequest.ParticipantId, permissionRequest.PermissionType);
        await _budgetPermissionRepository.UpdateAsync(budgetPermission, cancellationToken);
    }
}
