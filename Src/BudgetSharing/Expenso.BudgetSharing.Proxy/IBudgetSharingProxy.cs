using Expenso.BudgetSharing.Proxy.DTO.API.AssignOwnerPermission.Responses;
using Expenso.BudgetSharing.Proxy.DTO.API.GetBudgetPermissions.Responses;

namespace Expenso.BudgetSharing.Proxy;

public interface IBudgetSharingProxy
{
    Task<IReadOnlyCollection<GetBudgetPermissionsResponse>?> GetBudgetPermissionsAsync(Guid budgetId,
        CancellationToken cancellationToken = default);

    Task<AssignOwnerPermissionResponse?> AssignOwnerPermission(Guid budgetPermissionId, Guid budgetId, Guid ownerId,
        CancellationToken cancellationToken = default);
}