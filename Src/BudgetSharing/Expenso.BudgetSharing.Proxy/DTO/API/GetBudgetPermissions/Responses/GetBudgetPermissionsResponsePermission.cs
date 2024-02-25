namespace Expenso.BudgetSharing.Proxy.DTO.API.GetBudgetPermissions.Responses;

public sealed record GetBudgetPermissionsResponsePermission(
    Guid ParticipantId,
    GetBudgetPermissionsResponsePermissionType PermissionType);