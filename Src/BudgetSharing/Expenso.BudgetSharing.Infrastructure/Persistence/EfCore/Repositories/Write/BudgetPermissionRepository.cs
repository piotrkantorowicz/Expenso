using Expenso.BudgetSharing.Domain.BudgetPermissions;
using Expenso.BudgetSharing.Domain.BudgetPermissions.Repositories;
using Expenso.BudgetSharing.Domain.BudgetPermissions.ValueObjects;
using Expenso.BudgetSharing.Domain.Shared.ValueObjects;

using Microsoft.EntityFrameworkCore;

namespace Expenso.BudgetSharing.Infrastructure.Persistence.EfCore.Repositories.Write;

internal sealed class BudgetPermissionRepository(IBudgetSharingDbContext budgetSharingDbContext)
    : IBudgetPermissionRepository
{
    private readonly IBudgetSharingDbContext _budgetSharingDbContext =
        budgetSharingDbContext ?? throw new ArgumentNullException(nameof(budgetSharingDbContext));

    public async Task<BudgetPermission?> GetByIdAsync(BudgetPermissionId id, CancellationToken cancellationToken)
    {
        return await _budgetSharingDbContext
            .BudgetPermissions.IgnoreQueryFilters()
            .Include(x => x.Permissions)
            .SingleOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<BudgetPermission?> GetByBudgetIdAsync(BudgetId budgetId, CancellationToken cancellationToken)
    {
        return await _budgetSharingDbContext
            .BudgetPermissions.IgnoreQueryFilters()
            .Include(x => x.Permissions)
            .SingleOrDefaultAsync(x => x.BudgetId == budgetId, cancellationToken);
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
}