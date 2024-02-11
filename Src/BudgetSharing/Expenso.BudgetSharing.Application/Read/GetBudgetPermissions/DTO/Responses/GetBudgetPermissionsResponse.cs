namespace Expenso.BudgetSharing.Application.Read.GetBudgetPermissions.DTO.Responses;

public record GetBudgetPermissionsResponse(
    Guid Id,
    Guid BudgetId,
    Guid OwnerId,
    ICollection<GetBudgetPermissionsResponsePermission> Permissions);