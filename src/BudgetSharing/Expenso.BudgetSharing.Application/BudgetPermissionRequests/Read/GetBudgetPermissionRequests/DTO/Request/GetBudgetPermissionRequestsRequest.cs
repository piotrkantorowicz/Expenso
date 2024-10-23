namespace Expenso.BudgetSharing.Application.BudgetPermissionRequests.Read.GetBudgetPermissionRequests.DTO.Request;

public sealed record GetBudgetPermissionRequestsRequest(
    Guid? BudgetId = null,
    Guid? ParticipantId = null,
    Guid? OwnerId = null,
    bool? ForCurrentUser = null,
    GetBudgetPermissionRequestsRequest_Status? Status = null,
    GetBudgetPermissionRequestsRequest_PermissionType? PermissionType = null);