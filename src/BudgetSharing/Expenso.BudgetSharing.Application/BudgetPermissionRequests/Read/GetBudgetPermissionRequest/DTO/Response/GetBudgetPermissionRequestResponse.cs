namespace Expenso.BudgetSharing.Application.BudgetPermissionRequests.Read.GetBudgetPermissionRequest.DTO.Response;

public sealed record GetBudgetPermissionRequestResponse(
    Guid Id,
    Guid BudgetId,
    Guid ParticipantId,
    GetBudgetPermissionRequestResponse_PermissionType PermissionRequestType,
    GetBudgetPermissionRequestResponse_Status Status,
    DateTimeOffset ExpirationDate,
    DateTimeOffset SubmissionDate,
    DateTimeOffset? CancellationDate,
    DateTimeOffset? ConfirmationDate);