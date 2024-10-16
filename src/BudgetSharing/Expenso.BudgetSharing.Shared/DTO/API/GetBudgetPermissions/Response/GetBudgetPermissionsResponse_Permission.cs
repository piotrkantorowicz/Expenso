namespace Expenso.BudgetSharing.Shared.DTO.API.GetBudgetPermissions.Response;

public sealed record GetBudgetPermissionsResponse_Permission(
    Guid ParticipantId,
    GetBudgetPermissionsResponse_PermissionType PermissionType);