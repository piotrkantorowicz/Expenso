namespace Expenso.BudgetSharing.Proxy.DTO.API.GetBudgetPermissions.Response;

public sealed record GetBudgetPermissionsResponse(
    Guid Id,
    Guid BudgetId,
    Guid OwnerId,
    ICollection<GetBudgetPermissionsResponse_Permission> Permissions);