using Expenso.BudgetSharing.Domain.BudgetPermissionRequests.Model;
using Expenso.BudgetSharing.Domain.BudgetPermissionRequests.Model.ValueObjects;

namespace Expenso.BudgetSharing.Domain.BudgetPermissionRequests.Repositories;

public interface IBudgetPermissionRequestRepository
{
    Task<BudgetPermissionRequest?> GetByIdAsync(BudgetPermissionRequestId permissionId, bool useTracking,
        CancellationToken cancellationToken = default);

    Task<BudgetPermissionRequest> AddAsync(BudgetPermissionRequest permission, CancellationToken cancellationToken);

    Task<BudgetPermissionRequest> UpdateAsync(BudgetPermissionRequest permission, CancellationToken cancellationToken);
}
