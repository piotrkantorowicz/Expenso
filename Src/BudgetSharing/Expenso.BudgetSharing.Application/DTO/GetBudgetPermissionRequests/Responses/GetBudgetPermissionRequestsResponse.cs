namespace Expenso.BudgetSharing.Application.DTO.GetBudgetPermissionRequests.Responses;

public sealed record GetBudgetPermissionRequestsResponse(
    Guid Id,
    Guid BudgetId,
    Guid ParticipantId,
    GetBudgetPermissionRequestsResponsePermissionType GetBudgetPermissionRequestsResponsePermissionType,
    GetBudgetPermissionRequestsResponseStatus ResponseStatus,
    DateTimeOffset? ExpirationDate);