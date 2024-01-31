using Expenso.BudgetSharing.Application.QueryStore.Filters;
using Expenso.BudgetSharing.Domain.BudgetPermissionRequests.Model;

namespace Expenso.BudgetSharing.Application.QueryStore.Repositories;

public interface IBudgetPermissionRequestReadRepository
{
    Task<BudgetPermissionRequest> SingleAsync(BudgetPermissionRequestFilter filter,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<BudgetPermissionRequest>> Browse(BudgetPermissionRequestFilter filter,
        CancellationToken cancellationToken = default);
}