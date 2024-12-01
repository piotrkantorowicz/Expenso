namespace Expenso.BudgetSharing.Application.BudgetPermissionRequests.Read.GetBudgetPermissionRequest.DTO.Response;

public sealed record GetBudgetPermissionRequestResponse(
    Guid Id,
    Guid BudgetId,
    Guid ParticipantId,
    string BudgetCode,
    GetBudgetPermissionRequestResponsePermissionType PermissionType,
    GetBudgetPermissionRequestResponseStatus Status,
    DateTimeOffset ExpirationDate,
    DateTimeOffset SubmissionDate,
    DateTimeOffset? CancellationDate,
    DateTimeOffset? ConfirmationDate);