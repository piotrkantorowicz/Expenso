namespace Expenso.BudgetSharing.Application.BudgetPermissions.Read.GetBudgetPermission.DTO.Response;

public sealed record GetBudgetPermissionResponsePermission(
    Guid ParticipantId,
    GetBudgetPermissionResponsePermissionType PermissionType);