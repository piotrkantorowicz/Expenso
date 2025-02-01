namespace Expenso.BudgetSharing.Shared.DTO.API.BudgetPermissionRequests.AssignParticipant.Request;

public sealed record AssignParticipantRequest(
    Guid BudgetId,
    string Email,
    AssignParticipantRequestPermissionType PermissionType,
    Guid? BudgetPermissionRequestId = null);