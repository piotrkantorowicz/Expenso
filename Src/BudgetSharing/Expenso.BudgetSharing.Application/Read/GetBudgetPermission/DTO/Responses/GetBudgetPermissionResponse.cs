namespace Expenso.BudgetSharing.Application.Read.GetBudgetPermission.DTO.Responses;

public record GetBudgetPermissionResponse(
    Guid Id,
    Guid BudgetId,
    Guid OwnerId,
    ICollection<GetBudgetPermissionResponsePermission> Permissions);