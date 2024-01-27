using System.Linq.Expressions;

using Expenso.BudgetSharing.Domain.BudgetPermissions.Model;
using Expenso.BudgetSharing.Domain.BudgetPermissions.Model.ValueObjects;

namespace Expenso.BudgetSharing.Domain.BudgetPermissions.Repositories;

public interface IBudgetPermissionRepository
{
    Task<BudgetPermission?> GetByIdAsync(BudgetPermissionId id, bool includePermissions, bool useTracking,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<BudgetPermission>> FindAsync(Expression<Func<BudgetPermission, bool>> filter,
        bool includePermissions, bool useTracking, CancellationToken cancellationToken = default);

    Task<BudgetPermission> AddAsync(BudgetPermission permission, CancellationToken cancellationToken);

    Task<BudgetPermission> UpdateAsync(BudgetPermission permission, CancellationToken cancellationToken);

    Task<int> RemoveAsync(BudgetPermission permission, CancellationToken cancellationToken);
}
