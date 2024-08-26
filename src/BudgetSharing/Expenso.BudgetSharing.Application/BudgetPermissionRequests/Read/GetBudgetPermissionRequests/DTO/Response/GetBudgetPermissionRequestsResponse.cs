namespace Expenso.BudgetSharing.Application.BudgetPermissionRequests.Read.GetBudgetPermissionRequests.DTO.Response;

public sealed record GetBudgetPermissionRequestsResponse(
    Guid Id,
    Guid BudgetId,
    Guid ParticipantId,
    GetBudgetPermissionRequestsResponse_PermissionType PermissionType,
    GetBudgetPermissionRequestsResponse_Status Status,
    DateTimeOffset ExpirationDate,
    DateTimeOffset SubmissionDate,
    DateTimeOffset? CancellationDate,
    DateTimeOffset? ConfirmationDate);