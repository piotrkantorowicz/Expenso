using Expenso.BudgetSharing.Core.Domain.BudgetPermissionRequests.Model;
using Expenso.BudgetSharing.Core.Domain.BudgetPermissionRequests.Model.ValueObjects;
using Expenso.BudgetSharing.Core.Domain.BudgetPermissionRequests.Repositories;

namespace Expenso.BudgetSharing.Core.Persistence.EfCore.Repositories;

internal sealed class BudgetPermissionRequestRepository : IBudgetPermissionRequestRepository
{
    public Task<BudgetPermissionRequest?> GetByIdAsync(BudgetPermissionRequestId permissionId, bool useTracking,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<BudgetPermissionRequest> AddAsync(BudgetPermissionRequest permission,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<BudgetPermissionRequest> UpdateAsync(BudgetPermissionRequest permission,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}