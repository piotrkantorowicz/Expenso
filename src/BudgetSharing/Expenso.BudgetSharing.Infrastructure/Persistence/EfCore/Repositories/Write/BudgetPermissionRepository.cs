using Expenso.BudgetSharing.Domain.BudgetPermissions;
using Expenso.BudgetSharing.Domain.BudgetPermissions.Repositories;
using Expenso.BudgetSharing.Domain.BudgetPermissions.ValueObjects;
using Expenso.BudgetSharing.Domain.Shared.ValueObjects;

using Microsoft.EntityFrameworkCore;

namespace Expenso.BudgetSharing.Infrastructure.Persistence.EfCore.Repositories.Write;

internal sealed class BudgetPermissionRepository(IBudgetSharingDbContext budgetSharingDbContext)
    : IBudgetPermissionRepository
{
    private readonly IBudgetSharingDbContext _budgetSharingDbContext = budgetSharingDbContext ??
                                                                       throw new ArgumentNullException(
                                                                           paramName: nameof(budgetSharingDbContext));

    public async Task<BudgetPermission?> GetByIdAsync(BudgetPermissionId id, CancellationToken cancellationToken)
    {
        return await _budgetSharingDbContext
            .BudgetPermissions.IgnoreQueryFilters()
            .Include(navigationPropertyPath: x => x.Permissions)
            .SingleOrDefaultAsync(predicate: x => x.Id == id, cancellationToken: cancellationToken);
    }

    public async Task<BudgetPermission?> GetByBudgetIdAsync(BudgetId budgetId, CancellationToken cancellationToken)
    {
        return await _budgetSharingDbContext
            .BudgetPermissions.IgnoreQueryFilters()
            .Include(navigationPropertyPath: x => x.Permissions)
            .SingleOrDefaultAsync(predicate: x => x.BudgetId == budgetId, cancellationToken: cancellationToken);
    }

    public async Task AddOrUpdateAsync(BudgetPermission budgetPermission, CancellationToken cancellationToken)
    {
        if (_budgetSharingDbContext.GetEntryState(entity: budgetPermission) == EntityState.Detached)
        {
            await _budgetSharingDbContext.BudgetPermissions.AddAsync(entity: budgetPermission,
                cancellationToken: cancellationToken);
        }
        else
        {
            _budgetSharingDbContext.BudgetPermissions.Update(entity: budgetPermission);
        }

        await _budgetSharingDbContext.SaveChangesAsync(cancellationToken: cancellationToken);
    }

    public async Task UpdateAsync(BudgetPermission budgetPermission, CancellationToken cancellationToken)
    {
        _budgetSharingDbContext.BudgetPermissions.Update(entity: budgetPermission);
        await _budgetSharingDbContext.SaveChangesAsync(cancellationToken: cancellationToken);
    }
}