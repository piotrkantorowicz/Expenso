using Expenso.BudgetSharing.Application.QueryStore.Filters;
using Expenso.BudgetSharing.Domain.BudgetPermissions.Model;

namespace Expenso.BudgetSharing.Application.QueryStore.Repositories;

public interface IBudgetPermissionReadRepository
{
    Task<BudgetPermission> SingleAsync(BudgetPermissionFilter filter, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<BudgetPermission>> BrowseAsync(BudgetPermissionFilter filter,
        CancellationToken cancellationToken = default);
}