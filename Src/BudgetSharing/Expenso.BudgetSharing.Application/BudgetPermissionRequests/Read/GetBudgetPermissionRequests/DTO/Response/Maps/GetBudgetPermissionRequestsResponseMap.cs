using Expenso.BudgetSharing.Domain.BudgetPermissionRequests;
using Expenso.BudgetSharing.Domain.BudgetPermissionRequests.ValueObjects;
using Expenso.BudgetSharing.Domain.Shared.ValueObjects;

namespace Expenso.BudgetSharing.Application.BudgetPermissionRequests.Read.GetBudgetPermissionRequests.DTO.Response.Maps;

internal static class GetBudgetPermissionRequestsResponseMap
{
    public static IReadOnlyCollection<GetBudgetPermissionRequestsResponse> MapTo(
        IEnumerable<BudgetPermissionRequest> budgetPermissionRequests)
    {
        return budgetPermissionRequests.Select(MapTo).ToList();
    }

    private static GetBudgetPermissionRequestsResponse MapTo(BudgetPermissionRequest budgetPermissionRequest)
    {
        return new GetBudgetPermissionRequestsResponse(budgetPermissionRequest.Id.Value,
            budgetPermissionRequest.BudgetId.Value, budgetPermissionRequest.ParticipantId.Value,
            MapTo(budgetPermissionRequest.PermissionType), MapTo(budgetPermissionRequest.Status),
            budgetPermissionRequest.ExpirationDate?.Value);
    }

    private static GetBudgetPermissionRequestsResponse_Status MapTo(
        BudgetPermissionRequestStatus budgetPermissionRequestStatus)
    {
        if (budgetPermissionRequestStatus == BudgetPermissionRequestStatus.Unknown)
        {
            return GetBudgetPermissionRequestsResponse_Status.Unknown;
        }

        if (budgetPermissionRequestStatus == BudgetPermissionRequestStatus.Pending)
        {
            return GetBudgetPermissionRequestsResponse_Status.Pending;
        }

        if (budgetPermissionRequestStatus == BudgetPermissionRequestStatus.Cancelled)
        {
            return GetBudgetPermissionRequestsResponse_Status.Cancelled;
        }

        if (budgetPermissionRequestStatus == BudgetPermissionRequestStatus.Confirmed)
        {
            return GetBudgetPermissionRequestsResponse_Status.Confirmed;
        }

        if (budgetPermissionRequestStatus == BudgetPermissionRequestStatus.Expired)
        {
            return GetBudgetPermissionRequestsResponse_Status.Expired;
        }

        throw new ArgumentOutOfRangeException(nameof(budgetPermissionRequestStatus), budgetPermissionRequestStatus,
            null);
    }

    private static GetBudgetPermissionRequestsResponse_PermissionType MapTo(PermissionType permissionType)
    {
        if (permissionType == PermissionType.Unknown)
        {
            return GetBudgetPermissionRequestsResponse_PermissionType.Unknown;
        }

        if (permissionType == PermissionType.Owner)
        {
            return GetBudgetPermissionRequestsResponse_PermissionType.Owner;
        }

        if (permissionType == PermissionType.SubOwner)
        {
            return GetBudgetPermissionRequestsResponse_PermissionType.SubOwner;
        }

        if (permissionType == PermissionType.Reviewer)
        {
            return GetBudgetPermissionRequestsResponse_PermissionType.Reviewer;
        }

        throw new ArgumentOutOfRangeException(nameof(permissionType), permissionType, null);
    }
}