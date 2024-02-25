using Expenso.BudgetSharing.Domain.BudgetPermissionRequests.ValueObjects;
using Expenso.BudgetSharing.Domain.Shared.Model.ValueObjects;

namespace Expenso.BudgetSharing.Application.BudgetPermissionRequests.Read.GetBudgetPermissionRequests.DTO.Request.Maps;

public sealed class GetBudgetPermissionRequestsRequestMap
{
    public static PermissionType MapTo(GetBudgetPermissionRequestsRequest_PermissionType permissionType)
    {
        return permissionType switch
        {
            GetBudgetPermissionRequestsRequest_PermissionType.Unknown => PermissionType.Unknown,
            GetBudgetPermissionRequestsRequest_PermissionType.Owner => PermissionType.Owner,
            GetBudgetPermissionRequestsRequest_PermissionType.SubOwner => PermissionType.SubOwner,
            GetBudgetPermissionRequestsRequest_PermissionType.Reviewer => PermissionType.Reviewer,
            _ => throw new ArgumentOutOfRangeException(nameof(permissionType), permissionType, null)
        };
    }

    public static BudgetPermissionRequestStatus MapTo(GetBudgetPermissionRequestsRequest_Status status)
    {
        return status switch
        {
            GetBudgetPermissionRequestsRequest_Status.Unknown => BudgetPermissionRequestStatus.Unknown,
            GetBudgetPermissionRequestsRequest_Status.Pending => BudgetPermissionRequestStatus.Pending,
            GetBudgetPermissionRequestsRequest_Status.Cancelled => BudgetPermissionRequestStatus.Cancelled,
            GetBudgetPermissionRequestsRequest_Status.Confirmed => BudgetPermissionRequestStatus.Confirmed,
            GetBudgetPermissionRequestsRequest_Status.Expired => BudgetPermissionRequestStatus.Expired,
            _ => throw new ArgumentOutOfRangeException(nameof(status), status, null)
        };
    }
}