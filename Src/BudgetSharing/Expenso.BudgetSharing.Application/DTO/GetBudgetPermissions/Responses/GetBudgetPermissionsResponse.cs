namespace Expenso.BudgetSharing.Application.DTO.GetBudgetPermissions.Responses;

public record GetBudgetPermissionsResponse(
    Guid Id,
    Guid BudgetId,
    Guid OwnerId,
    ICollection<GetBudgetPermissionsResponsePermission> Permissions);