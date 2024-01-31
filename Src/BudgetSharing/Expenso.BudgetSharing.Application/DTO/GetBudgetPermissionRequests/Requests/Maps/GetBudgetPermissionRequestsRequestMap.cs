using Expenso.BudgetSharing.Domain.BudgetPermissionRequests.Model.ValueObjects;
using Expenso.BudgetSharing.Domain.Shared.Model.ValueObjects;

namespace Expenso.BudgetSharing.Application.DTO.GetBudgetPermissionRequests.Requests.Maps;

public sealed class GetBudgetPermissionRequestsRequestMap
{
    public static PermissionType MapTo(GetBudgetPermissionRequestsRequestPermissionType permissionType)
    {
        return permissionType switch
        {
            GetBudgetPermissionRequestsRequestPermissionType.Unknown => PermissionType.Unknown,
            GetBudgetPermissionRequestsRequestPermissionType.Owner => PermissionType.Owner,
            GetBudgetPermissionRequestsRequestPermissionType.SubOwner => PermissionType.SubOwner,
            GetBudgetPermissionRequestsRequestPermissionType.Reviewer => PermissionType.Reviewer,
            _ => throw new ArgumentOutOfRangeException(nameof(permissionType), permissionType, null)
        };
    }

    public static BudgetPermissionRequestStatus MapTo(GetBudgetPermissionRequestsRequestStatus status)
    {
        return status switch
        {
            GetBudgetPermissionRequestsRequestStatus.Unknown => BudgetPermissionRequestStatus.Unknown,
            GetBudgetPermissionRequestsRequestStatus.Pending => BudgetPermissionRequestStatus.Pending,
            GetBudgetPermissionRequestsRequestStatus.Cancelled => BudgetPermissionRequestStatus.Cancelled,
            GetBudgetPermissionRequestsRequestStatus.Confirmed => BudgetPermissionRequestStatus.Confirmed,
            GetBudgetPermissionRequestsRequestStatus.Expired => BudgetPermissionRequestStatus.Expired,
            _ => throw new ArgumentOutOfRangeException(nameof(status), status, null)
        };
    }
}