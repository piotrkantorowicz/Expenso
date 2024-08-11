using Expenso.BudgetSharing.Domain.BudgetPermissionRequests.ValueObjects;

namespace Expenso.BudgetSharing.Domain.BudgetPermissionRequests.Repositories;

public interface IBudgetPermissionRequestRepository
{
    Task<BudgetPermissionRequest?> GetByIdAsync(BudgetPermissionRequestId permissionId,
        CancellationToken cancellationToken);

    Task<BudgetPermissionRequest> AddAsync(BudgetPermissionRequest permission, CancellationToken cancellationToken);

    Task<BudgetPermissionRequest> UpdateAsync(BudgetPermissionRequest permission, CancellationToken cancellationToken);
}