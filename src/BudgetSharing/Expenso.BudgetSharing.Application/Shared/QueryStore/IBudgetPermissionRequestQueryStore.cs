using Expenso.BudgetSharing.Application.Shared.QueryStore.Filters;
using Expenso.BudgetSharing.Domain.BudgetPermissionRequests;
using Expenso.Shared.Database.Ordering;
using Expenso.Shared.Database.Paging;
using Expenso.Shared.System.Types.Paging;

namespace Expenso.BudgetSharing.Application.Shared.QueryStore;

public interface IBudgetPermissionRequestQueryStore
{
    Task<BudgetPermissionRequest?> SingleAsync(BudgetPermissionRequestQuerySpecification querySpecification,
        CancellationToken cancellationToken);

    Task<IPagedList<BudgetPermissionRequest>> BrowseAsync(BudgetPermissionRequestQuerySpecification querySpecification,
        DatabasePagination? pagination, DatabaseSorting? sorters, CancellationToken cancellationToken);
}