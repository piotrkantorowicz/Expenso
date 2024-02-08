using Expenso.BudgetSharing.Application.Read.Shared.QueryStore.Filters;
using Expenso.BudgetSharing.Domain.BudgetPermissions;

namespace Expenso.BudgetSharing.Application.Read.Shared.QueryStore;

public interface IBudgetPermissionQueryStore
{
    Task<BudgetPermission?> SingleAsync(BudgetPermissionFilter filter, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<BudgetPermission>> BrowseAsync(BudgetPermissionFilter filter,
        CancellationToken cancellationToken = default);
}