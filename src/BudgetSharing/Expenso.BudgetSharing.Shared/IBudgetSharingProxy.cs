using Expenso.BudgetSharing.Shared.DTO.API.CreateBudgetPermission.Request;
using Expenso.BudgetSharing.Shared.DTO.API.CreateBudgetPermission.Response;
using Expenso.BudgetSharing.Shared.DTO.API.GetBudgetPermissions.Response;

namespace Expenso.BudgetSharing.Shared;

public interface IBudgetSharingProxy
{
    Task<IReadOnlyCollection<GetBudgetPermissionsResponse>?> GetBudgetPermissionsAsync(Guid budgetId,
        CancellationToken cancellationToken = default);

    Task<CreateBudgetPermissionResponse?> CreateBudgetPermission(
        CreateBudgetPermissionRequest createBudgetPermissionRequest, CancellationToken cancellationToken = default);

    Task DeleteBudgetPermission(Guid budgetPermissionId, CancellationToken cancellationToken = default);

    Task RestoreBudgetPermission(Guid budgetPermissionId, CancellationToken cancellationToken = default);
}