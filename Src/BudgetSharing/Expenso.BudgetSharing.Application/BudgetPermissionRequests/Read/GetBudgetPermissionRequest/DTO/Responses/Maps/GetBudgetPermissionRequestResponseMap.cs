using Expenso.BudgetSharing.Domain.BudgetPermissionRequests;
using Expenso.BudgetSharing.Domain.BudgetPermissionRequests.ValueObjects;
using Expenso.BudgetSharing.Domain.Shared.Model.ValueObjects;

namespace Expenso.BudgetSharing.Application.BudgetPermissionRequests.Read.GetBudgetPermissionRequest.DTO.Responses.Maps;

internal static class GetBudgetPermissionRequestResponseMap
{
    public static GetBudgetPermissionRequestResponse MapTo(BudgetPermissionRequest budgetPermissionRequest)
    {
        return new GetBudgetPermissionRequestResponse(budgetPermissionRequest.Id.Value,
            budgetPermissionRequest.BudgetId.Value, budgetPermissionRequest.ParticipantId.Value,
            MapTo(budgetPermissionRequest.PermissionType), MapTo(budgetPermissionRequest.Status),
            budgetPermissionRequest.ExpirationDate?.Value);
    }

    private static GetBudgetPermissionRequestResponseStatus MapTo(
        BudgetPermissionRequestStatus budgetPermissionRequestStatus)
    {
        if (budgetPermissionRequestStatus == BudgetPermissionRequestStatus.Unknown)
        {
            return GetBudgetPermissionRequestResponseStatus.Unknown;
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

        throw new ArgumentOutOfRangeException(nameof(budgetPermissionRequestStatus), budgetPermissionRequestStatus,
            null);
    }

    private static GetBudgetPermissionRequestResponsePermissionType MapTo(PermissionType permissionType)
    {
        if (permissionType == PermissionType.Unknown)
        {
            return GetBudgetPermissionRequestResponsePermissionType.Unknown;
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

        throw new ArgumentOutOfRangeException(nameof(permissionType), permissionType, null);
    }
}