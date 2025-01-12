using Expenso.BudgetSharing.Application.Shared.QueryStore;
using Expenso.BudgetSharing.Application.Shared.QueryStore.Filters;
using Expenso.BudgetSharing.Domain.BudgetPermissions;
using Expenso.Shared.Database.EfCore.Queryable;
using Expenso.Shared.System.Types.Pagination;

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

    public async Task<BudgetPermission?> SingleAsync(BudgetPermissionQuerySpecification querySpecification,
        CancellationToken cancellationToken)
    {
        return await _budgetPermissionsQueryable
            .Where(predicate: querySpecification.Filter())
            .SingleOrDefaultAsync(cancellationToken: cancellationToken);
    }

    public async Task<IPagedList<BudgetPermission>> BrowseAsync(BudgetPermissionQuerySpecification querySpecification,
        CancellationToken cancellationToken)
    {
        return await _budgetPermissionsQueryable.PaginationAsync(filter: querySpecification.Filter(),
            pagination: querySpecification.Pagination, cancellationToken: cancellationToken);
    }
}