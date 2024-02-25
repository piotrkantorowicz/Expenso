namespace Expenso.BudgetSharing.Application.BudgetPermissionRequests.Read.GetBudgetPermissionRequest.DTO.Responses;

public sealed record GetBudgetPermissionRequestResponse(
    Guid Id,
    Guid BudgetId,
    Guid ParticipantId,
    GetBudgetPermissionRequestResponsePermissionType PermissionType,
    GetBudgetPermissionRequestResponseStatus Status,
    DateTimeOffset? ExpirationDate);