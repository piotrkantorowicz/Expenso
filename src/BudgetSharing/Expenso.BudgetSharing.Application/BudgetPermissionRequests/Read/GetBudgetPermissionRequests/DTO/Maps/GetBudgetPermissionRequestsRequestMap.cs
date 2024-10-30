using Expenso.BudgetSharing.Application.BudgetPermissionRequests.Read.GetBudgetPermissionRequests.DTO.Request;
using Expenso.BudgetSharing.Domain.BudgetPermissionRequests.ValueObjects;
using Expenso.BudgetSharing.Domain.Shared.ValueObjects;

namespace Expenso.BudgetSharing.Application.BudgetPermissionRequests.Read.GetBudgetPermissionRequests.DTO.Maps;

public sealed class GetBudgetPermissionRequestsRequestMap
{
    public static PermissionType? MapTo(GetBudgetPermissionRequestsRequest_PermissionType? permissionType)
    {
        return permissionType switch
        {
            GetBudgetPermissionRequestsRequest_PermissionType.None => PermissionType.None,
            GetBudgetPermissionRequestsRequest_PermissionType.Owner => PermissionType.Owner,
            GetBudgetPermissionRequestsRequest_PermissionType.SubOwner => PermissionType.SubOwner,
            GetBudgetPermissionRequestsRequest_PermissionType.Reviewer => PermissionType.Reviewer,
            _ => null
        };
    }

    public static BudgetPermissionRequestStatus? MapTo(GetBudgetPermissionRequestsRequest_Status? status)
    {
        return status switch
        {
            GetBudgetPermissionRequestsRequest_Status.None => BudgetPermissionRequestStatus.None,
            GetBudgetPermissionRequestsRequest_Status.Pending => BudgetPermissionRequestStatus.Pending,
            GetBudgetPermissionRequestsRequest_Status.Cancelled => BudgetPermissionRequestStatus.Cancelled,
            GetBudgetPermissionRequestsRequest_Status.Confirmed => BudgetPermissionRequestStatus.Confirmed,
            GetBudgetPermissionRequestsRequest_Status.Expired => BudgetPermissionRequestStatus.Expired,
            _ => null
        };
    }
}