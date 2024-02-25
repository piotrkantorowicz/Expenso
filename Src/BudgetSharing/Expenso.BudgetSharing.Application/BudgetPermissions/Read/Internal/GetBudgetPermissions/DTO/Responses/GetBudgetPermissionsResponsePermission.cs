namespace Expenso.BudgetSharing.Application.BudgetPermissions.Read.Internal.GetBudgetPermissions.DTO.Responses;

public sealed record GetBudgetPermissionsResponsePermission(
    Guid ParticipantId,
    GetBudgetPermissionsResponsePermissionType PermissionType);