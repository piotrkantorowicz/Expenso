using System.Linq.Expressions;

using Expenso.BudgetSharing.Domain.BudgetPermissions.Model;
using Expenso.BudgetSharing.Domain.BudgetPermissions.Model.ValueObjects;
using Expenso.BudgetSharing.Domain.BudgetPermissions.Repositories;
using Expenso.Shared.Database.EfCore.Extensions;

using Microsoft.EntityFrameworkCore;

namespace Expenso.BudgetSharing.Infrastructure.Persistence.EfCore.Repositories;

internal sealed class BudgetPermissionRepository(IBudgetSharingDbContext budgetSharingDbContext)
    : IBudgetPermissionRepository
{
    private readonly IBudgetSharingDbContext _budgetSharingDbContext =
        budgetSharingDbContext ?? throw new ArgumentNullException(nameof(budgetSharingDbContext));

    public async Task<BudgetPermission?> GetByIdAsync(BudgetPermissionId id, bool includePermissions, bool useTracking,
        CancellationToken cancellationToken = default)
    {
        return await _budgetSharingDbContext
            .BudgetPermissions.Tracking(useTracking)
            .Include(includePermissions, x => x.Permissions)
            .SingleOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<BudgetPermission>> FindAsync(Expression<Func<BudgetPermission, bool>> filter,
        bool includePermissions, bool useTracking, CancellationToken cancellationToken = default)
    {
        return await _budgetSharingDbContext
            .BudgetPermissions.Tracking(useTracking)
            .Include(includePermissions, x => x.Permissions)
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
