namespace Expenso.BudgetSharing.Application.BudgetPermissionRequests.Read.GetBudgetPermissionRequests.DTO.Responses;

public sealed record GetBudgetPermissionRequestsResponse(
    Guid Id,
    Guid BudgetId,
    Guid ParticipantId,
    GetBudgetPermissionRequestsResponsePermissionType PermissionType,
    GetBudgetPermissionRequestsResponseStatus Status,
    DateTimeOffset? ExpirationDate);