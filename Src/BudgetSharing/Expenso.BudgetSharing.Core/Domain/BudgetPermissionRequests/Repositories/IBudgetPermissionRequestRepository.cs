using Expenso.BudgetSharing.Core.Domain.BudgetPermissionRequests.Model;
using Expenso.BudgetSharing.Core.Domain.BudgetPermissionRequests.Model.ValueObjects;

namespace Expenso.BudgetSharing.Core.Domain.BudgetPermissionRequests.Repositories;

internal interface IBudgetPermissionRequestRepository
{
    Task<BudgetPermissionRequest?> GetByIdAsync(BudgetPermissionRequestId permissionId, bool useTracking,
        CancellationToken cancellationToken = default);

    Task<BudgetPermissionRequest> AddAsync(BudgetPermissionRequest permission, CancellationToken cancellationToken);

    Task<BudgetPermissionRequest> UpdateAsync(BudgetPermissionRequest permission, CancellationToken cancellationToken);
}