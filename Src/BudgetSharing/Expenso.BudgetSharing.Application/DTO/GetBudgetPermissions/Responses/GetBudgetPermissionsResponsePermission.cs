namespace Expenso.BudgetSharing.Application.DTO.GetBudgetPermissions.Responses;

public sealed record GetBudgetPermissionsResponsePermission(
    Guid Id,
    Guid BudgetPermissionId,
    Guid ParticipantId,
    GetBudgetPermissionsResponsePermissionType GetBudgetPermissionsResponsePermissionType);