using Expenso.BudgetSharing.Application.BudgetPermissionRequests.Read.GetBudgetPermissionRequests.DTO.Request;
using Expenso.BudgetSharing.Domain.BudgetPermissionRequests.ValueObjects;
using Expenso.BudgetSharing.Domain.Shared.ValueObjects;

namespace Expenso.BudgetSharing.Application.BudgetPermissionRequests.Read.GetBudgetPermissionRequests.DTO.Maps;

public static class GetBudgetPermissionRequestsRequestMap
{
    private static readonly Dictionary<GetBudgetPermissionRequestsRequestPermissionType, PermissionType>
        PermissionTypeMap = new()
        {
            { GetBudgetPermissionRequestsRequestPermissionType.Owner, PermissionType.Owner },
            { GetBudgetPermissionRequestsRequestPermissionType.SubOwner, PermissionType.SubOwner },
            { GetBudgetPermissionRequestsRequestPermissionType.Reviewer, PermissionType.Reviewer }
        };

    private static readonly Dictionary<GetBudgetPermissionRequestsRequestStatus, BudgetPermissionRequestStatus>
        StatusMap = new()
        {
            { GetBudgetPermissionRequestsRequestStatus.Pending, BudgetPermissionRequestStatus.Pending },
            { GetBudgetPermissionRequestsRequestStatus.Confirmed, BudgetPermissionRequestStatus.Confirmed },
            { GetBudgetPermissionRequestsRequestStatus.Cancelled, BudgetPermissionRequestStatus.Cancelled },
            { GetBudgetPermissionRequestsRequestStatus.Expired, BudgetPermissionRequestStatus.Expired }
        };

    public static PermissionType[] MapTo(GetBudgetPermissionRequestsRequestPermissionType? permissionType)
    {
        return permissionType is null or GetBudgetPermissionRequestsRequestPermissionType.None
            ? ( [PermissionType.None])
            : PermissionTypeMap
                .Where(predicate: kv => permissionType.Value.HasFlag(flag: kv.Key))
                .Select(selector: kv => kv.Value)
                .ToArray();
    }

    public static BudgetPermissionRequestStatus[] MapTo(GetBudgetPermissionRequestsRequestStatus? status)
    {
        return status is null or GetBudgetPermissionRequestsRequestStatus.None
            ? ( [BudgetPermissionRequestStatus.None])
            : StatusMap
                .Where(predicate: kv => status.Value.HasFlag(flag: kv.Key))
                .Select(selector: kv => kv.Value)
                .ToArray();
    }
}