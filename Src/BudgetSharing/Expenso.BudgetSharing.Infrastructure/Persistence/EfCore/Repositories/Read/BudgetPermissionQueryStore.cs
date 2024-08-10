using Expenso.BudgetSharing.Application.Shared.QueryStore;
using Expenso.BudgetSharing.Application.Shared.QueryStore.Filters;
using Expenso.BudgetSharing.Domain.BudgetPermissions;
using Expenso.BudgetSharing.Infrastructure.Persistence.EfCore.Extensions;
using Expenso.Shared.Database.EfCore.Queryable;

using Microsoft.EntityFrameworkCore;

namespace Expenso.BudgetSharing.Infrastructure.Persistence.EfCore.Repositories.Read;

internal sealed class BudgetPermissionQueryStore : IBudgetPermissionQueryStore
{
    private readonly IQueryable<BudgetPermission> _budgetPermissionsQueryable;

    public BudgetPermissionQueryStore(IBudgetSharingDbContext budgetSharingDbContext)
    {
        ArgumentNullException.ThrowIfNull(argument: budgetSharingDbContext);
        _budgetPermissionsQueryable = budgetSharingDbContext.BudgetPermissions.Tracking(useTracking: false);
    }

    public async Task<BudgetPermission?> SingleAsync(BudgetPermissionFilter filter, CancellationToken cancellationToken)
    {
        return await _budgetPermissionsQueryable
            .Where(predicate: filter.ToFilterExpression())
            .SingleOrDefaultAsync(cancellationToken: cancellationToken);
    }

    public async Task<IReadOnlyList<BudgetPermission>> BrowseAsync(BudgetPermissionFilter filter,
        CancellationToken cancellationToken)
    {
        return await _budgetPermissionsQueryable
            .Where(predicate: filter.ToFilterExpression())
            .ToListAsync(cancellationToken: cancellationToken);
    }
}