using Expenso.BudgetSharing.Shared.DTO.API.BudgetPermissions.GetBudgetPermissions.Request;
using Expenso.BudgetSharing.Shared.DTO.API.BudgetPermissions.GetBudgetPermissions.Response;

namespace Expenso.BudgetSharing.Shared;

public interface IBudgetSharingProxy
{
    Task<IReadOnlyCollection<GetBudgetPermissionsResponse>?> GetBudgetPermissionsAsync(
        GetBudgetPermissionsRequest request, CancellationToken cancellationToken = default);
}