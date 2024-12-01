namespace Expenso.BudgetSharing.Shared.DTO.API.BudgetPermissions.GetBudgetPermissions.Response;

public sealed record GetBudgetPermissionsResponse(
    Guid Id,
    Guid BudgetId,
    Guid OwnerId,
    string BudgetCode,
    ICollection<GetBudgetPermissionsResponsePermission> Permissions);