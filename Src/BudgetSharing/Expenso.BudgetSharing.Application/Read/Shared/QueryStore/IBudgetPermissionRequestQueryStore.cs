using Expenso.BudgetSharing.Application.Read.Shared.QueryStore.Filters;
using Expenso.BudgetSharing.Domain.BudgetPermissionRequests;

namespace Expenso.BudgetSharing.Application.Read.Shared.QueryStore;

public interface IBudgetPermissionRequestQueryStore
{
    Task<BudgetPermissionRequest?> SingleAsync(BudgetPermissionRequestFilter filter,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<BudgetPermissionRequest>> Browse(BudgetPermissionRequestFilter filter,
        CancellationToken cancellationToken = default);
}