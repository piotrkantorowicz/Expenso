using Expenso.BudgetSharing.Application.Shared.QueryStore.Filters;
using Expenso.BudgetSharing.Domain.BudgetPermissions;
using Expenso.Shared.Database.Ordering;
using Expenso.Shared.Database.Paging;
using Expenso.Shared.System.Types.Paging;

namespace Expenso.BudgetSharing.Application.Shared.QueryStore;

public interface IBudgetPermissionQueryStore
{
    Task<BudgetPermission?> SingleAsync(BudgetPermissionQuerySpecification querySpecification,
        CancellationToken cancellationToken);

    Task<IPagedList<BudgetPermission>> BrowseAsync(BudgetPermissionQuerySpecification querySpecification,
        DatabasePagination? pagination, DatabaseSorting? sorters,
        CancellationToken cancellationToken);
}