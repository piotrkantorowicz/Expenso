using Expenso.BudgetSharing.Core.Domain.BudgetPermissionRequests.Model;
using Expenso.BudgetSharing.Core.Domain.BudgetPermissionRequests.Model.ValueObjects;
using Expenso.BudgetSharing.Core.Domain.BudgetPermissionRequests.Repositories;
using Expenso.BudgetSharing.Core.Domain.BudgetPermissions.Model;
using Expenso.BudgetSharing.Core.Domain.BudgetPermissions.Model.ValueObjects;
using Expenso.BudgetSharing.Core.Domain.BudgetPermissions.Repositories;
using Expenso.Shared.Types.Exceptions;

namespace Expenso.BudgetSharing.Core.Domain.BudgetPermissions.Services;

internal sealed class BudgetPermissionService
{
    private readonly IBudgetPermissionRepository _budgetPermissionRepository;
    private readonly IBudgetPermissionRequestRepository _budgetPermissionRequestRepository;

    public BudgetPermissionService(IBudgetPermissionRepository budgetPermissionRepository,
        IBudgetPermissionRequestRepository budgetPermissionRequestRepository)
    {
        _budgetPermissionRepository = budgetPermissionRepository ??
                                      throw new ArgumentNullException(nameof(budgetPermissionRepository));

        _budgetPermissionRequestRepository = budgetPermissionRequestRepository ??
                                             throw new ArgumentNullException(nameof(budgetPermissionRequestRepository));
    }

    public async Task<BudgetPermission> AcceptBudgetPermissionAsync(BudgetPermissionRequestId budgetPermissionRequestId,
        BudgetPermissionId budgetPermissionId, CancellationToken cancellationToken)
    {
        BudgetPermissionRequest? permissionRequest =
            await _budgetPermissionRequestRepository.GetByIdAsync(budgetPermissionRequestId, true, cancellationToken);

        if (permissionRequest is null)
        {
            throw new NotFoundException($"Budget permission request with id {budgetPermissionRequestId} not found.");
        }

        permissionRequest.Confirm();
        await _budgetPermissionRequestRepository.UpdateAsync(permissionRequest, cancellationToken);

        BudgetPermission? budgetPermission = await _budgetPermissionRepository.GetByIdAsync(budgetPermissionId, true,
            cancellationToken);

        if (budgetPermission is null)
        {
            throw new NotFoundException($"Budget permission with id {budgetPermissionId} not found.");
        }

        budgetPermission.AddPermission(permissionRequest.ParticipantId, permissionRequest.PermissionType);
        await _budgetPermissionRepository.UpdateAsync(budgetPermission, cancellationToken);

        return budgetPermission;
    }
}