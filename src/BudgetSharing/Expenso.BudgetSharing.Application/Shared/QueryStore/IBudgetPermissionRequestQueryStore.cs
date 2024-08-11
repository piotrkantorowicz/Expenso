using Expenso.BudgetSharing.Application.Shared.QueryStore.Filters;
using Expenso.BudgetSharing.Domain.BudgetPermissionRequests;

namespace Expenso.BudgetSharing.Application.Shared.QueryStore;

public interface IBudgetPermissionRequestQueryStore
{
    Task<BudgetPermissionRequest?> SingleAsync(BudgetPermissionRequestFilter filter,
        CancellationToken cancellationToken);

    Task<IReadOnlyCollection<BudgetPermissionRequest>> Browse(BudgetPermissionRequestFilter filter,
        CancellationToken cancellationToken);
}