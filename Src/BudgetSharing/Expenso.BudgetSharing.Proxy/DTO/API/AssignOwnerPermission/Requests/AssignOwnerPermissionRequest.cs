namespace Expenso.BudgetSharing.Proxy.DTO.API.AssignOwnerPermission.Requests;

public sealed record AssignOwnerPermissionRequest(Guid BudgetId, Guid OwnerId);