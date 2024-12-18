using Expenso.BudgetSharing.Application.Shared.QueryStore.Filters;
using Expenso.BudgetSharing.Domain.BudgetPermissionRequests;
using Expenso.Shared.System.Types.Pagination;

namespace Expenso.BudgetSharing.Application.Shared.QueryStore;

public interface IBudgetPermissionRequestQueryStore
{
    Task<BudgetPermissionRequest?> SingleAsync(BudgetPermissionRequestFilter filter,
        CancellationToken cancellationToken);

    Task<IPagedList<BudgetPermissionRequest>> Browse(BudgetPermissionRequestFilter filter,
        CancellationToken cancellationToken);
}