namespace Expenso.BudgetSharing.Application.BudgetPermissionRequests.Read.GetBudgetPermissionRequest.DTO.Response;

public sealed record GetBudgetPermissionRequestResponse(
    Guid Id,
    Guid BudgetId,
    Guid ParticipantId,
    GetBudgetPermissionRequestResponsePermissionType PermissionType,
    GetBudgetPermissionRequestResponseStatus Status,
    DateTimeOffset ExpirationDate,
    DateTimeOffset SubmissionDate,
    DateTimeOffset? CancellationDate,
    DateTimeOffset? ConfirmationDate);