namespace Expenso.BudgetSharing.Application.Read.GetBudgetPermissions.DTO.Responses;

public sealed record GetBudgetPermissionsResponsePermission(
    Guid ParticipantId,
    GetBudgetPermissionsResponsePermissionType GetBudgetPermissionsResponsePermissionType);