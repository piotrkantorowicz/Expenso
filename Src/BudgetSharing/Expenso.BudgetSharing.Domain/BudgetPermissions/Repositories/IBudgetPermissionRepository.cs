using Expenso.BudgetSharing.Domain.BudgetPermissions.Model;
using Expenso.BudgetSharing.Domain.BudgetPermissions.Model.ValueObjects;

namespace Expenso.BudgetSharing.Domain.BudgetPermissions.Repositories;

public interface IBudgetPermissionRepository
{
    Task<BudgetPermission?> GetByIdAsync(BudgetPermissionId id, CancellationToken cancellationToken = default);

    Task<BudgetPermission> AddOrUpdateAsync(BudgetPermission budgetPermission, CancellationToken cancellationToken);

    Task<BudgetPermission> UpdateAsync(BudgetPermission budgetPermission, CancellationToken cancellationToken);

    Task<int> RemoveAsync(BudgetPermission budgetPermission, CancellationToken cancellationToken);
}