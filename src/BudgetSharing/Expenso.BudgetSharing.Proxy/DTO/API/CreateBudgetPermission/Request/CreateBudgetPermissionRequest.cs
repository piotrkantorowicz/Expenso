namespace Expenso.BudgetSharing.Proxy.DTO.API.CreateBudgetPermission.Request;

public sealed record CreateBudgetPermissionRequest(Guid? BudgetPermissionId, Guid BudgetId, Guid OwnerId);