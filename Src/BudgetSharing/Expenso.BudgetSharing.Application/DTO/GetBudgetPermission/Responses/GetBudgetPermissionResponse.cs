namespace Expenso.BudgetSharing.Application.DTO.GetBudgetPermission.Responses;

public record GetBudgetPermissionResponse(
    Guid Id,
    Guid BudgetId,
    Guid OwnerId,
    ICollection<GetBudgetPermissionResponsePermission> Permissions);