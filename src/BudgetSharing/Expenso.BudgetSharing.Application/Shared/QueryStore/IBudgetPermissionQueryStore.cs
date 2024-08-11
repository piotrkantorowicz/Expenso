using Expenso.BudgetSharing.Application.Shared.QueryStore.Filters;
using Expenso.BudgetSharing.Domain.BudgetPermissions;

namespace Expenso.BudgetSharing.Application.Shared.QueryStore;

public interface IBudgetPermissionQueryStore
{
    Task<BudgetPermission?> SingleAsync(BudgetPermissionFilter filter, CancellationToken cancellationToken);

    Task<IReadOnlyList<BudgetPermission>> BrowseAsync(BudgetPermissionFilter filter,
        CancellationToken cancellationToken);
}