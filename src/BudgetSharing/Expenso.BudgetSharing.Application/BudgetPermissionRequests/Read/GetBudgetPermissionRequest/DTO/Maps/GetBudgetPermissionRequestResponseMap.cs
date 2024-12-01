using Expenso.BudgetSharing.Application.BudgetPermissionRequests.Read.GetBudgetPermissionRequest.DTO.Response;
using Expenso.BudgetSharing.Domain.BudgetPermissionRequests;
using Expenso.BudgetSharing.Domain.BudgetPermissionRequests.ValueObjects;
using Expenso.BudgetSharing.Domain.Shared.ValueObjects;

namespace Expenso.BudgetSharing.Application.BudgetPermissionRequests.Read.GetBudgetPermissionRequest.DTO.Maps;

internal static class GetBudgetPermissionRequestResponseMap
{
    public static GetBudgetPermissionRequestResponse MapTo(BudgetPermissionRequest budgetPermissionRequest)
    {
        return new GetBudgetPermissionRequestResponse(Id: budgetPermissionRequest.Id.Value,
            BudgetId: budgetPermissionRequest.BudgetId.Value,
            ParticipantId: budgetPermissionRequest.ParticipantId.Value,
            BudgetCode: budgetPermissionRequest.BudgetCode.Value,
            PermissionType: MapTo(permissionType: budgetPermissionRequest.PermissionType),
            Status: MapTo(budgetPermissionRequestStatus: budgetPermissionRequest.StatusTracker.Status),
            ExpirationDate: budgetPermissionRequest.StatusTracker.ExpirationDate.Value,
            SubmissionDate: budgetPermissionRequest.StatusTracker.SubmissionDate.Value,
            CancellationDate: budgetPermissionRequest.StatusTracker.CancellationDate?.Value,
            ConfirmationDate: budgetPermissionRequest.StatusTracker.ConfirmationDate?.Value);
    }

    private static GetBudgetPermissionRequestResponseStatus MapTo(
        BudgetPermissionRequestStatus budgetPermissionRequestStatus)
    {
        if (budgetPermissionRequestStatus == BudgetPermissionRequestStatus.None)
        {
            return GetBudgetPermissionRequestResponseStatus.None;
        }

        if (budgetPermissionRequestStatus == BudgetPermissionRequestStatus.Pending)
        {
            return GetBudgetPermissionRequestResponseStatus.Pending;
        }

        if (budgetPermissionRequestStatus == BudgetPermissionRequestStatus.Cancelled)
        {
            return GetBudgetPermissionRequestResponseStatus.Cancelled;
        }

        if (budgetPermissionRequestStatus == BudgetPermissionRequestStatus.Confirmed)
        {
            return GetBudgetPermissionRequestResponseStatus.Confirmed;
        }

        if (budgetPermissionRequestStatus == BudgetPermissionRequestStatus.Expired)
        {
            return GetBudgetPermissionRequestResponseStatus.Expired;
        }

        throw new ArgumentOutOfRangeException(paramName: nameof(budgetPermissionRequestStatus),
            actualValue: budgetPermissionRequestStatus, message: null);
    }

    private static GetBudgetPermissionRequestResponsePermissionType MapTo(PermissionType permissionType)
    {
        if (permissionType == PermissionType.None)
        {
            return GetBudgetPermissionRequestResponsePermissionType.None;
        }

        if (permissionType == PermissionType.Owner)
        {
            return GetBudgetPermissionRequestResponsePermissionType.Owner;
        }

        if (permissionType == PermissionType.SubOwner)
        {
            return GetBudgetPermissionRequestResponsePermissionType.SubOwner;
        }

        if (permissionType == PermissionType.Reviewer)
        {
            return GetBudgetPermissionRequestResponsePermissionType.Reviewer;
        }

        throw new ArgumentOutOfRangeException(paramName: nameof(permissionType), actualValue: permissionType,
            message: null);
    }
}