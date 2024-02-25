namespace Expenso.BudgetSharing.Application.BudgetPermissions.Read.Internal.GetBudgetPermission.DTO.Responses;

public sealed record GetBudgetPermissionResponsePermission(
    Guid ParticipantId,
    GetBudgetPermissionResponsePermissionType PermissionType);