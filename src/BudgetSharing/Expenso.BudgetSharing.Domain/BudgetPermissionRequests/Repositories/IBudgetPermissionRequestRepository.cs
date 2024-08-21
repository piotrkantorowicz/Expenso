using Expenso.BudgetSharing.Domain.BudgetPermissionRequests.ValueObjects;
using Expenso.BudgetSharing.Domain.Shared.ValueObjects;

namespace Expenso.BudgetSharing.Domain.BudgetPermissionRequests.Repositories;

public interface IBudgetPermissionRequestRepository
{
    Task<BudgetPermissionRequest?> GetByIdAsync(BudgetPermissionRequestId permissionId,
        CancellationToken cancellationToken);

    Task<IReadOnlyCollection<BudgetPermissionRequest>> GetUncompletedByPersonIdAsync(BudgetId budgetId,
        PersonId participantId, CancellationToken cancellationToken);

    Task<BudgetPermissionRequest> AddAsync(BudgetPermissionRequest permission, CancellationToken cancellationToken);

    Task<BudgetPermissionRequest> UpdateAsync(BudgetPermissionRequest permission, CancellationToken cancellationToken);
}