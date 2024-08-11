namespace Expenso.BudgetSharing.Application.BudgetPermissions.Read.GetBudgetPermission.DTO.Response;

public sealed record GetBudgetPermissionResponse_Permission(
    Guid ParticipantId,
    GetBudgetPermissionResponse_PermissionType PermissionType);