using Expenso.BudgetSharing.Domain.BudgetPermissions.ValueObjects;
using Expenso.BudgetSharing.Domain.Shared.ValueObjects;

namespace Expenso.BudgetSharing.Domain.BudgetPermissions.Repositories;

public interface IBudgetPermissionRepository
{
    Task<BudgetPermission?> GetByIdAsync(BudgetPermissionId id, CancellationToken cancellationToken);

    Task<BudgetPermission?> GetByBudgetIdAsync(BudgetId budgetId, CancellationToken cancellationToken);

    Task<BudgetPermission> AddOrUpdateAsync(BudgetPermission budgetPermission, CancellationToken cancellationToken);

    Task<BudgetPermission> UpdateAsync(BudgetPermission budgetPermission, CancellationToken cancellationToken);
}