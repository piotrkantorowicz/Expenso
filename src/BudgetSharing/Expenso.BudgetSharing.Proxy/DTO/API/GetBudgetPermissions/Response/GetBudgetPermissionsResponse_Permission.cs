namespace Expenso.BudgetSharing.Proxy.DTO.API.GetBudgetPermissions.Response;

public sealed record GetBudgetPermissionsResponse_Permission(
    Guid ParticipantId,
    GetBudgetPermissionsResponse_PermissionType PermissionType);