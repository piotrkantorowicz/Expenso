namespace Expenso.BudgetSharing.Application.DTO.GetBudgetPermission.Responses;

public sealed record GetBudgetPermissionResponsePermission(
    Guid Id,
    Guid BudgetPermissionId,
    Guid ParticipantId,
    GetBudgetPermissionResponsePermissionType GetBudgetPermissionResponsePermissionType);