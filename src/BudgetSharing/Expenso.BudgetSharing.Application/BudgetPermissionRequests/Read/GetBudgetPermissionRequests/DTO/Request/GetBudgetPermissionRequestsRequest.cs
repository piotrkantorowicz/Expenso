namespace Expenso.BudgetSharing.Application.BudgetPermissionRequests.Read.GetBudgetPermissionRequests.DTO.Request;

public sealed record GetBudgetPermissionRequestsRequest(
    Guid? BudgetId = null,
    string? BudgetCode = null,
    Guid? ParticipantId = null,
    Guid? OwnerId = null,
    bool? ForCurrentUser = null,
    GetBudgetPermissionRequestsRequestStatus Status = GetBudgetPermissionRequestsRequestStatus.All,
    GetBudgetPermissionRequestsRequestPermissionType PermissionType =
        GetBudgetPermissionRequestsRequestPermissionType.All);