using Expenso.BudgetSharing.Application.BudgetPermissionRequests.Read.GetBudgetPermissionRequests.DTO.Request;
using Expenso.BudgetSharing.Domain.BudgetPermissionRequests.ValueObjects;
using Expenso.BudgetSharing.Domain.Shared.ValueObjects;

namespace Expenso.BudgetSharing.Application.BudgetPermissionRequests.Read.GetBudgetPermissionRequests.DTO.Maps;

public static class GetBudgetPermissionRequestsRequestMap
{
    private static readonly Dictionary<GetBudgetPermissionRequestsRequest_PermissionType, PermissionType>
        PermissionTypeMap = new()
        {
            { GetBudgetPermissionRequestsRequest_PermissionType.Owner, PermissionType.Owner },
            { GetBudgetPermissionRequestsRequest_PermissionType.SubOwner, PermissionType.SubOwner },
            { GetBudgetPermissionRequestsRequest_PermissionType.Reviewer, PermissionType.Reviewer }
        };

    private static readonly Dictionary<GetBudgetPermissionRequestsRequest_Status, BudgetPermissionRequestStatus>
        StatusMap = new()
        {
            { GetBudgetPermissionRequestsRequest_Status.Pending, BudgetPermissionRequestStatus.Pending },
            { GetBudgetPermissionRequestsRequest_Status.Confirmed, BudgetPermissionRequestStatus.Confirmed },
            { GetBudgetPermissionRequestsRequest_Status.Cancelled, BudgetPermissionRequestStatus.Cancelled },
            { GetBudgetPermissionRequestsRequest_Status.Expired, BudgetPermissionRequestStatus.Expired }
        };

    public static PermissionType[] MapTo(GetBudgetPermissionRequestsRequest_PermissionType? permissionType)
    {
        return permissionType is null or GetBudgetPermissionRequestsRequest_PermissionType.None
            ? ( [PermissionType.None])
            : PermissionTypeMap
                .Where(predicate: kv => permissionType.Value.HasFlag(flag: kv.Key))
                .Select(selector: kv => kv.Value)
                .ToArray();
    }

    public static BudgetPermissionRequestStatus[] MapTo(GetBudgetPermissionRequestsRequest_Status? status)
    {
        return status is null or GetBudgetPermissionRequestsRequest_Status.None
            ? ( [BudgetPermissionRequestStatus.None])
            : StatusMap
                .Where(predicate: kv => status.Value.HasFlag(flag: kv.Key))
                .Select(selector: kv => kv.Value)
                .ToArray();
    }
}