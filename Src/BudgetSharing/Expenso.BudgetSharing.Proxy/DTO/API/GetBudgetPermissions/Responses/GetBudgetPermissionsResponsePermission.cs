namespace Expenso.BudgetSharing.Proxy.DTO.API.GetBudgetPermissions.Responses;

public sealed record GetBudgetPermissionsResponsePermission(
    Guid Id,
    Guid BudgetPermissionId,
    Guid ParticipantId,
    GetBudgetPermissionsResponsePermissionType GetBudgetPermissionsResponsePermissionType);