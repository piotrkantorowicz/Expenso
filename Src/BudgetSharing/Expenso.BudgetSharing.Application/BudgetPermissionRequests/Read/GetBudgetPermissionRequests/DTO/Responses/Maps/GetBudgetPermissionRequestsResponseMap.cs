using Expenso.BudgetSharing.Domain.BudgetPermissionRequests;
using Expenso.BudgetSharing.Domain.BudgetPermissionRequests.ValueObjects;
using Expenso.BudgetSharing.Domain.Shared.Model.ValueObjects;

namespace Expenso.BudgetSharing.Application.BudgetPermissionRequests.Read.GetBudgetPermissionRequests.DTO.Responses.
    Maps;

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

    private static GetBudgetPermissionRequestsResponseStatus MapTo(
        BudgetPermissionRequestStatus budgetPermissionRequestStatus)
    {
        if (budgetPermissionRequestStatus == BudgetPermissionRequestStatus.Unknown)
        {
            return GetBudgetPermissionRequestsResponseStatus.Unknown;
        }

        if (budgetPermissionRequestStatus == BudgetPermissionRequestStatus.Pending)
        {
            return GetBudgetPermissionRequestsResponseStatus.Pending;
        }

        if (budgetPermissionRequestStatus == BudgetPermissionRequestStatus.Cancelled)
        {
            return GetBudgetPermissionRequestsResponseStatus.Cancelled;
        }

        if (budgetPermissionRequestStatus == BudgetPermissionRequestStatus.Confirmed)
        {
            return GetBudgetPermissionRequestsResponseStatus.Confirmed;
        }

        if (budgetPermissionRequestStatus == BudgetPermissionRequestStatus.Expired)
        {
            return GetBudgetPermissionRequestsResponseStatus.Expired;
        }

        throw new ArgumentOutOfRangeException(nameof(budgetPermissionRequestStatus), budgetPermissionRequestStatus,
            null);
    }

    private static GetBudgetPermissionRequestsResponsePermissionType MapTo(PermissionType permissionType)
    {
        if (permissionType == PermissionType.Unknown)
        {
            return GetBudgetPermissionRequestsResponsePermissionType.Unknown;
        }

        if (permissionType == PermissionType.Owner)
        {
            return GetBudgetPermissionRequestsResponsePermissionType.Owner;
        }

        if (permissionType == PermissionType.SubOwner)
        {
            return GetBudgetPermissionRequestsResponsePermissionType.SubOwner;
        }

        if (permissionType == PermissionType.Reviewer)
        {
            return GetBudgetPermissionRequestsResponsePermissionType.Reviewer;
        }

        throw new ArgumentOutOfRangeException(nameof(permissionType), permissionType, null);
    }
}