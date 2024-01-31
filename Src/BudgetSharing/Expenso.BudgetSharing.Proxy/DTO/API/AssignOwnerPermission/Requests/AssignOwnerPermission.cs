namespace Expenso.BudgetSharing.Proxy.DTO.API.AssignOwnerPermission.Requests;

public sealed record AssignOwnerPermission(Guid BudgetId, Guid OwnerId);