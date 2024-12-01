namespace Expenso.BudgetSharing.Application.BudgetPermissions.Read.GetBudgetPermission.DTO.Response;

public record GetBudgetPermissionResponse(
    Guid Id,
    Guid BudgetId,
    Guid OwnerId,
    string BudgetCode,
    ICollection<GetBudgetPermissionResponsePermission> Permissions);