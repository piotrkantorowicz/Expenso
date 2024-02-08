namespace Expenso.BudgetSharing.Application.Read.GetBudgetPermissionRequests.DTO.Responses;

public sealed record GetBudgetPermissionRequestsResponse(
    Guid Id,
    Guid BudgetId,
    Guid ParticipantId,
    GetBudgetPermissionRequestsResponsePermissionType GetBudgetPermissionRequestsResponsePermissionType,
    GetBudgetPermissionRequestsResponseStatus ResponseStatus,
    DateTimeOffset? ExpirationDate);