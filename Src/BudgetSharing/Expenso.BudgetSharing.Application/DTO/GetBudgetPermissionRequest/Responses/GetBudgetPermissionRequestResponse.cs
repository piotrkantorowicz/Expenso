namespace Expenso.BudgetSharing.Application.DTO.GetBudgetPermissionRequest.Responses;

public sealed record GetBudgetPermissionRequestResponse(
    Guid Id,
    Guid BudgetId,
    Guid ParticipantId,
    GetBudgetPermissionRequestResponsePermissionType GetBudgetPermissionRequestResponsePermissionType,
    GetBudgetPermissionRequestResponseStatus ResponseStatus,
    DateTimeOffset? ExpirationDate);