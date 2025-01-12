using Expenso.BudgetSharing.Application.Shared.QueryStore.Filters;
using Expenso.BudgetSharing.Domain.BudgetPermissionRequests;
using Expenso.Shared.System.Types.Pagination;

namespace Expenso.BudgetSharing.Application.Shared.QueryStore;

public interface IBudgetPermissionRequestQueryStore
{
    Task<BudgetPermissionRequest?> SingleAsync(BudgetPermissionRequestQuerySpecification querySpecification,
        CancellationToken cancellationToken);

    Task<IPagedList<BudgetPermissionRequest>> BrowseAsync(BudgetPermissionRequestQuerySpecification querySpecification,
        CancellationToken cancellationToken);
}