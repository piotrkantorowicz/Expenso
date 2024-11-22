using Expenso.BudgetSharing.Domain.BudgetPermissions.ValueObjects;
using Expenso.BudgetSharing.Domain.Shared.ValueObjects;

namespace Expenso.BudgetSharing.Domain.BudgetPermissions.Repositories;

public interface IBudgetPermissionRepository
{
    Task<BudgetPermission?> GetByIdAsync(BudgetPermissionId budgetPermissionId, CancellationToken cancellationToken);

    Task<BudgetPermission?> GetByBudgetIdAsync(BudgetId budgetId, CancellationToken cancellationToken);

    Task<bool> IsUnique(BudgetPermissionId budgetPermissionId, BudgetId budgetId, PersonId ownerId,
        BudgetCode budgetCode, CancellationToken cancellationToken);

    Task AddOrUpdateAsync(BudgetPermission budgetPermission, CancellationToken cancellationToken);

    Task UpdateAsync(BudgetPermission budgetPermission, CancellationToken cancellationToken);
}