namespace Expenso.BudgetSharing.Application.BudgetPermissionRequests.Read.GetBudgetPermissionRequests.DTO.Response;

public sealed record GetBudgetPermissionRequestsResponse(
    Guid Id,
    Guid BudgetId,
    Guid ParticipantId,
    string BudgetCode,
    GetBudgetPermissionRequestsResponsePermissionType PermissionType,
    GetBudgetPermissionRequestsResponseStatus Status,
    DateTimeOffset ExpirationDate,
    DateTimeOffset SubmissionDate,
    DateTimeOffset? CancellationDate,
    DateTimeOffset? ConfirmationDate);