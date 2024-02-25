namespace Expenso.BudgetSharing.Application.BudgetPermissions.Read.Internal.GetBudgetPermissions.DTO.Responses;

public record GetBudgetPermissionsResponse(
    Guid Id,
    Guid BudgetId,
    Guid OwnerId,
    ICollection<GetBudgetPermissionsResponsePermission> Permissions);