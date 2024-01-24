using System.Linq.Expressions;

using Expenso.BudgetSharing.Core.Domain.BudgetPermissions.Model;
using Expenso.BudgetSharing.Core.Domain.BudgetPermissions.Model.ValueObjects;
using Expenso.BudgetSharing.Core.Domain.BudgetPermissions.Repositories;
using Expenso.Shared.Database.EfCore.Extensions;

using Microsoft.EntityFrameworkCore;

namespace Expenso.BudgetSharing.Core.Persistence.EfCore.Repositories;

internal sealed class BudgetPermissionRepository(IBudgetSharingDbContext budgetSharingDbContext)
    : IBudgetPermissionRepository
{
    private readonly IBudgetSharingDbContext _budgetSharingDbContext =
        budgetSharingDbContext ?? throw new ArgumentNullException(nameof(budgetSharingDbContext));

    public async Task<BudgetPermission?> GetByIdAsync(BudgetPermissionId permissionId, bool useTracking,
        CancellationToken cancellationToken = default)
    {
        return await _budgetSharingDbContext
            .BudgetPermissions.Tracking(useTracking)
            .SingleOrDefaultAsync(x => x.Id == permissionId, cancellationToken);
    }

    public async Task<IReadOnlyList<BudgetPermission>> FindAsync(Expression<Func<BudgetPermission, bool>> filter,
        bool useTracking, CancellationToken cancellationToken = default)
    {
        return await _budgetSharingDbContext
            .BudgetPermissions.Tracking(useTracking)
            .Where(filter)
            .ToListAsync(cancellationToken);
    }

    public async Task<BudgetPermission> AddAsync(BudgetPermission permission, CancellationToken cancellationToken)
    {
        await _budgetSharingDbContext.BudgetPermissions.AddAsync(permission, cancellationToken);
        await _budgetSharingDbContext.SaveChangesAsync(cancellationToken);

        return permission;
    }

    public async Task<BudgetPermission> UpdateAsync(BudgetPermission permission, CancellationToken cancellationToken)
    {
        _budgetSharingDbContext.BudgetPermissions.Update(permission);
        await _budgetSharingDbContext.SaveChangesAsync(cancellationToken);

        return permission;
    }

    public async Task<int> RemoveAsync(BudgetPermission permission, CancellationToken cancellationToken)
    {
        _budgetSharingDbContext.BudgetPermissions.Remove(permission);

        return await _budgetSharingDbContext.SaveChangesAsync(cancellationToken);
    }
}