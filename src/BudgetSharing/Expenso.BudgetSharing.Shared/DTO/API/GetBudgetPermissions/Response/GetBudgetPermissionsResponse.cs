namespace Expenso.BudgetSharing.Shared.DTO.API.GetBudgetPermissions.Response;

public sealed record GetBudgetPermissionsResponse(
    Guid Id,
    Guid BudgetId,
    Guid OwnerId,
    ICollection<GetBudgetPermissionsResponse_Permission> Permissions);