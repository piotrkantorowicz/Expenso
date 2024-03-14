using Expenso.BudgetSharing.Proxy.DTO.API.CreateBudgetPermission.Request;
using Expenso.BudgetSharing.Proxy.DTO.API.CreateBudgetPermission.Response;
using Expenso.BudgetSharing.Proxy.DTO.API.GetBudgetPermissions.Response;

namespace Expenso.BudgetSharing.Proxy;

public interface IBudgetSharingProxy
{
    Task<IReadOnlyCollection<GetBudgetPermissionsResponse>?> GetBudgetPermissionsAsync(Guid budgetId,
        CancellationToken cancellationToken = default);

    Task<CreateBudgetPermissionResponse?> CreateBudgetPermission(
        CreateBudgetPermissionRequest createBudgetPermissionRequest, CancellationToken cancellationToken = default);

    Task DeleteBudgetPermission(Guid budgetPermissionId, CancellationToken cancellationToken = default);

    Task RestoreBudgetPermission(Guid budgetPermissionId, CancellationToken cancellationToken = default);
}