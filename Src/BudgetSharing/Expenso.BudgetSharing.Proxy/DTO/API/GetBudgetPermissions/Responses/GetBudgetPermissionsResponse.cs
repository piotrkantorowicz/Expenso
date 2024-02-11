namespace Expenso.BudgetSharing.Proxy.DTO.API.GetBudgetPermissions.Responses;

public record GetBudgetPermissionsResponse(
    Guid Id,
    Guid BudgetId,
    Guid OwnerId,
    ICollection<GetBudgetPermissionsResponsePermission> Permissions);