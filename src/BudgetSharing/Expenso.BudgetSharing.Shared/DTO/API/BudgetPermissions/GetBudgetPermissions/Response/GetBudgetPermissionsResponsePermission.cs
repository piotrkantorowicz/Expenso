namespace Expenso.BudgetSharing.Shared.DTO.API.BudgetPermissions.GetBudgetPermissions.Response;

public sealed record GetBudgetPermissionsResponsePermission(
    Guid ParticipantId,
    GetBudgetPermissionsResponsePermissionType PermissionType);