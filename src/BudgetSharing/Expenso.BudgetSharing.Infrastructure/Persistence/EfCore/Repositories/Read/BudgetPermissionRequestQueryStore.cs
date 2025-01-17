using Expenso.BudgetSharing.Application.Shared.QueryStore;
using Expenso.BudgetSharing.Application.Shared.QueryStore.Filters;
using Expenso.BudgetSharing.Domain.BudgetPermissionRequests;
using Expenso.Shared.Database.EfCore.Collections;
using Expenso.Shared.System.Types.Ordering;
using Expenso.Shared.System.Types.Pagination;

using Microsoft.EntityFrameworkCore;

namespace Expenso.BudgetSharing.Infrastructure.Persistence.EfCore.Repositories.Read;

internal sealed class BudgetPermissionRequestQueryStore : IBudgetPermissionRequestQueryStore
{
    private readonly IQueryable<BudgetPermissionRequest> _budgetPermissionRequestsQueryable;

    public BudgetPermissionRequestQueryStore(IBudgetSharingDbContext budgetSharingDbContext)
    {
        ArgumentNullException.ThrowIfNull(argument: budgetSharingDbContext);

        _budgetPermissionRequestsQueryable =
            budgetSharingDbContext.BudgetPermissionRequests.Tracking(useTracking: false);
    }

    public async Task<BudgetPermissionRequest?> SingleAsync(
        BudgetPermissionRequestQuerySpecification querySpecification,
        CancellationToken cancellationToken)
    {
        return await _budgetPermissionRequestsQueryable
            .Where(predicate: querySpecification.Filter())
            .SingleOrDefaultAsync(cancellationToken: cancellationToken);
    }

    public async Task<IPagedList<BudgetPermissionRequest>> BrowseAsync(
        BudgetPermissionRequestQuerySpecification querySpecification, Paging? pagination, Sorting? sorters,
        CancellationToken cancellationToken)
    {
        return await _budgetPermissionRequestsQueryable
            .ApplySorting(sorting: sorters)
            .PaginationAsync(filter: querySpecification.Filter(), pagination: pagination,
                cancellationToken: cancellationToken);
    }
}