using Expenso.BudgetSharing.Application.BudgetPermissionRequests.Read.GetBudgetPermissionRequests.DTO.Request;
using Expenso.BudgetSharing.Domain.BudgetPermissionRequests.ValueObjects;
using Expenso.BudgetSharing.Domain.Shared.ValueObjects;

namespace Expenso.BudgetSharing.Application.BudgetPermissionRequests.Read.GetBudgetPermissionRequests.DTO.Maps;

public static class GetBudgetPermissionRequestsRequestMap
{
    public static PermissionType[] MapTo(GetBudgetPermissionRequestsRequest_PermissionType? permissionType)
    {
        if (permissionType is null or GetBudgetPermissionRequestsRequest_PermissionType.None)
        {
            return [PermissionType.None];
        }

        ICollection<PermissionType> permissionTypes = [];

        if (permissionType.Value.HasFlag(flag: GetBudgetPermissionRequestsRequest_PermissionType.Owner))
        {
            permissionTypes.Add(item: PermissionType.Owner);
        }

        if (permissionType.Value.HasFlag(flag: GetBudgetPermissionRequestsRequest_PermissionType.SubOwner))
        {
            permissionTypes.Add(item: PermissionType.SubOwner);
        }

        if (permissionType.Value.HasFlag(flag: GetBudgetPermissionRequestsRequest_PermissionType.Reviewer))
        {
            permissionTypes.Add(item: PermissionType.Reviewer);
        }

        return permissionTypes.ToArray();
    }

    public static BudgetPermissionRequestStatus[] MapTo(GetBudgetPermissionRequestsRequest_Status? status)
    {
        if (status is null or GetBudgetPermissionRequestsRequest_Status.None)
        {
            return [BudgetPermissionRequestStatus.None];
        }

        ICollection<BudgetPermissionRequestStatus> budgetPermissionRequestStatus = [];

        if (status.Value.HasFlag(flag: GetBudgetPermissionRequestsRequest_Status.Pending))
        {
            budgetPermissionRequestStatus.Add(item: BudgetPermissionRequestStatus.Pending);
        }

        if (status.Value.HasFlag(flag: GetBudgetPermissionRequestsRequest_Status.Confirmed))
        {
            budgetPermissionRequestStatus.Add(item: BudgetPermissionRequestStatus.Confirmed);
        }

        if (status.Value.HasFlag(flag: GetBudgetPermissionRequestsRequest_Status.Cancelled))
        {
            budgetPermissionRequestStatus.Add(item: BudgetPermissionRequestStatus.Cancelled);
        }

        if (status.Value.HasFlag(flag: GetBudgetPermissionRequestsRequest_Status.Expired))
        {
            budgetPermissionRequestStatus.Add(item: BudgetPermissionRequestStatus.Expired);
        }

        return budgetPermissionRequestStatus.ToArray();
    }
}