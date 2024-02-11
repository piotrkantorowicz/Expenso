namespace Expenso.BudgetSharing.Application.Read.GetBudgetPermissionRequest.DTO.Responses;

public sealed record GetBudgetPermissionRequestResponse(
    Guid Id,
    Guid BudgetId,
    Guid ParticipantId,
    GetBudgetPermissionRequestResponsePermissionType GetBudgetPermissionRequestResponsePermissionType,
    GetBudgetPermissionRequestResponseStatus ResponseStatus,
    DateTimeOffset? ExpirationDate);