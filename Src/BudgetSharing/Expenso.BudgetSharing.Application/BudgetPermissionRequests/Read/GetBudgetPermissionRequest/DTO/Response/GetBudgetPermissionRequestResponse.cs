namespace Expenso.BudgetSharing.Application.BudgetPermissionRequests.Read.GetBudgetPermissionRequest.DTO.Response;

public sealed record GetBudgetPermissionRequestResponse(
    Guid Id,
    Guid BudgetId,
    Guid ParticipantId,
    GetBudgetPermissionRequestResponse_PermissionType PermissionType,
    GetBudgetPermissionRequestResponse_Status Status,
    DateTimeOffset? ExpirationDate);