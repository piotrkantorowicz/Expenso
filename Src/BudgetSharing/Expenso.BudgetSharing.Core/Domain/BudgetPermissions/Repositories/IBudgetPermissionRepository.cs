using System.Linq.Expressions;

using Expenso.BudgetSharing.Core.Domain.BudgetPermissions.Model;
using Expenso.BudgetSharing.Core.Domain.BudgetPermissions.Model.ValueObjects;

namespace Expenso.BudgetSharing.Core.Domain.BudgetPermissions.Repositories;

internal interface IBudgetPermissionRepository
{
    Task<BudgetPermission?> GetByIdAsync(BudgetPermissionId permissionId, bool useTracking,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<BudgetPermission>> FindAsync(Expression<Func<BudgetPermission, bool>> filter, bool useTracking,
        CancellationToken cancellationToken = default);

    Task<BudgetPermission> AddAsync(BudgetPermission permission, CancellationToken cancellationToken);

    Task<BudgetPermission> UpdateAsync(BudgetPermission permission, CancellationToken cancellationToken);

    Task<int> RemoveAsync(BudgetPermission permission, CancellationToken cancellationToken);
}