using Expenso.BudgetSharing.Domain.BudgetPermissions.Model;
using Expenso.BudgetSharing.Domain.BudgetPermissions.Model.ValueObjects;
using Expenso.BudgetSharing.Domain.BudgetPermissions.Repositories;

using Microsoft.EntityFrameworkCore;

namespace Expenso.BudgetSharing.Infrastructure.Persistence.EfCore.Repositories;

internal sealed class BudgetPermissionRepository(IBudgetSharingDbContext budgetSharingDbContext)
    : IBudgetPermissionRepository
{
    private readonly IBudgetSharingDbContext _budgetSharingDbContext =
        budgetSharingDbContext ?? throw new ArgumentNullException(nameof(budgetSharingDbContext));

    public async Task<BudgetPermission?> GetByIdAsync(BudgetPermissionId id,
        CancellationToken cancellationToken = default)
    {
        return await _budgetSharingDbContext
            .BudgetPermissions.Include(x => x.Permissions)
            .SingleOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<BudgetPermission> AddOrUpdateAsync(BudgetPermission permission,
        CancellationToken cancellationToken)
    {
        if (_budgetSharingDbContext.GetEntryState(permission) == EntityState.Detached)
        {
            await _budgetSharingDbContext.BudgetPermissions.AddAsync(permission, cancellationToken);
        }
        else
        {
            _budgetSharingDbContext.BudgetPermissions.Update(permission);
        }

        await _budgetSharingDbContext.SaveChangesAsync(cancellationToken);

        return permission;
    }

    public async Task<BudgetPermission> UpdateAsync(BudgetPermission budgetPermission,
        CancellationToken cancellationToken)
    {
        _budgetSharingDbContext.BudgetPermissions.Update(budgetPermission);
        await _budgetSharingDbContext.SaveChangesAsync(cancellationToken);

        return budgetPermission;
    }

    public async Task<int> RemoveAsync(BudgetPermission budgetPermission, CancellationToken cancellationToken)
    {
        _budgetSharingDbContext.BudgetPermissions.Remove(budgetPermission);

        return await _budgetSharingDbContext.SaveChangesAsync(cancellationToken);
    }
}