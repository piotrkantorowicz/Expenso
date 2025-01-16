using Expenso.BudgetSharing.Application.Shared.QueryStore.Filters;
using Expenso.BudgetSharing.Domain.BudgetPermissions;
using Expenso.Shared.System.Types.Pagination;

namespace Expenso.BudgetSharing.Application.Shared.QueryStore;

public interface IBudgetPermissionQueryStore
{
    Task<BudgetPermission?> SingleAsync(BudgetPermissionQuerySpecification querySpecification,
        CancellationToken cancellationToken);

    Task<IPagedList<BudgetPermission>> BrowseAsync(BudgetPermissionQuerySpecification querySpecification,
        CancellationToken cancellationToken);
}