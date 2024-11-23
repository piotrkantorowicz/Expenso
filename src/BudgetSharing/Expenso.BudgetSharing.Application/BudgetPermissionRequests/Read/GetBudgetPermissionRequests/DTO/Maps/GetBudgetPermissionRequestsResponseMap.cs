using Expenso.BudgetSharing.Application.BudgetPermissionRequests.Read.GetBudgetPermissionRequests.DTO.Response;
using Expenso.BudgetSharing.Domain.BudgetPermissionRequests;
using Expenso.BudgetSharing.Domain.BudgetPermissionRequests.ValueObjects;
using Expenso.BudgetSharing.Domain.Shared.ValueObjects;

namespace Expenso.BudgetSharing.Application.BudgetPermissionRequests.Read.GetBudgetPermissionRequests.DTO.Maps;

internal static class GetBudgetPermissionRequestsResponseMap
{
    public static IReadOnlyCollection<GetBudgetPermissionRequestsResponse> MapTo(
        IEnumerable<BudgetPermissionRequest> budgetPermissionRequests)
    {
        return budgetPermissionRequests.Select(selector: MapTo).ToList();
    }

    private static GetBudgetPermissionRequestsResponse MapTo(BudgetPermissionRequest budgetPermissionRequest)
    {
        return new GetBudgetPermissionRequestsResponse(Id: budgetPermissionRequest.Id.Value,
            BudgetId: budgetPermissionRequest.BudgetId.Value,
            ParticipantId: budgetPermissionRequest.ParticipantId.Value,
            PermissionType: MapTo(permissionType: budgetPermissionRequest.PermissionType),
            Status: MapTo(budgetPermissionRequestStatus: budgetPermissionRequest.StatusTracker.Status),
            ExpirationDate: budgetPermissionRequest.StatusTracker.ExpirationDate.Value,
            SubmissionDate: budgetPermissionRequest.StatusTracker.SubmissionDate.Value,
            CancellationDate: budgetPermissionRequest.StatusTracker.CancellationDate?.Value,
            ConfirmationDate: budgetPermissionRequest.StatusTracker.ConfirmationDate?.Value);
    }

    private static GetBudgetPermissionRequestsResponseStatus MapTo(
        BudgetPermissionRequestStatus budgetPermissionRequestStatus)
    {
        if (budgetPermissionRequestStatus == BudgetPermissionRequestStatus.None)
        {
            return GetBudgetPermissionRequestsResponseStatus.None;
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

        throw new ArgumentOutOfRangeException(paramName: nameof(budgetPermissionRequestStatus),
            actualValue: budgetPermissionRequestStatus, message: null);
    }

    private static GetBudgetPermissionRequestsResponsePermissionType MapTo(PermissionType permissionType)
    {
        if (permissionType == PermissionType.None)
        {
            return GetBudgetPermissionRequestsResponsePermissionType.None;
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

        throw new ArgumentOutOfRangeException(paramName: nameof(permissionType), actualValue: permissionType,
            message: null);
    }
}