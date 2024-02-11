namespace Expenso.BudgetSharing.Application.Read.GetBudgetPermission.DTO.Responses;

public sealed record GetBudgetPermissionResponsePermission(
    Guid ParticipantId,
    GetBudgetPermissionResponsePermissionType GetBudgetPermissionResponsePermissionType);