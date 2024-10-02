using Expenso.BudgetSharing.Domain.BudgetPermissionRequests;
using Expenso.BudgetSharing.Domain.BudgetPermissionRequests.ValueObjects;
using Expenso.BudgetSharing.Domain.Shared.ValueObjects;

namespace Expenso.BudgetSharing.Application.BudgetPermissionRequests.Read.GetBudgetPermissionRequest.DTO.Response.Maps;

internal static class GetBudgetPermissionRequestResponseMap
{
    public static GetBudgetPermissionRequestResponse MapTo(BudgetPermissionRequest budgetPermissionRequest)
    {
        return new GetBudgetPermissionRequestResponse(Id: budgetPermissionRequest.Id.Value,
            BudgetId: budgetPermissionRequest.BudgetId.Value,
            ParticipantId: budgetPermissionRequest.ParticipantId.Value,
            PermissionRequestType: MapTo(permissionType: budgetPermissionRequest.PermissionType),
            Status: MapTo(budgetPermissionRequestStatus: budgetPermissionRequest.StatusTracker.Status),
            ExpirationDate: budgetPermissionRequest.StatusTracker.ExpirationDate.Value,
            SubmissionDate: budgetPermissionRequest.StatusTracker.SubmissionDate.Value,
            CancellationDate: budgetPermissionRequest.StatusTracker.CancellationDate?.Value,
            ConfirmationDate: budgetPermissionRequest.StatusTracker.ConfirmationDate?.Value);
    }

    private static GetBudgetPermissionRequestResponse_Status MapTo(
        BudgetPermissionRequestStatus budgetPermissionRequestStatus)
    {
        if (budgetPermissionRequestStatus == BudgetPermissionRequestStatus.Unknown)
        {
            return GetBudgetPermissionRequestResponse_Status.Unknown;
        }

        if (budgetPermissionRequestStatus == BudgetPermissionRequestStatus.Pending)
        {
            return GetBudgetPermissionRequestResponse_Status.Pending;
        }

        if (budgetPermissionRequestStatus == BudgetPermissionRequestStatus.Cancelled)
        {
            return GetBudgetPermissionRequestResponse_Status.Cancelled;
        }

        if (budgetPermissionRequestStatus == BudgetPermissionRequestStatus.Confirmed)
        {
            return GetBudgetPermissionRequestResponse_Status.Confirmed;
        }

        if (budgetPermissionRequestStatus == BudgetPermissionRequestStatus.Expired)
        {
            return GetBudgetPermissionRequestResponse_Status.Expired;
        }

        throw new ArgumentOutOfRangeException(paramName: nameof(budgetPermissionRequestStatus),
            actualValue: budgetPermissionRequestStatus, message: null);
    }

    private static GetBudgetPermissionRequestResponse_PermissionType MapTo(PermissionType permissionType)
    {
        if (permissionType == PermissionType.Unknown)
        {
            return GetBudgetPermissionRequestResponse_PermissionType.Unknown;
        }

        if (permissionType == PermissionType.Owner)
        {
            return GetBudgetPermissionRequestResponse_PermissionType.Owner;
        }

        if (permissionType == PermissionType.SubOwner)
        {
            return GetBudgetPermissionRequestResponse_PermissionType.SubOwner;
        }

        if (permissionType == PermissionType.Reviewer)
        {
            return GetBudgetPermissionRequestResponse_PermissionType.Reviewer;
        }

        throw new ArgumentOutOfRangeException(paramName: nameof(permissionType), actualValue: permissionType,
            message: null);
    }
}