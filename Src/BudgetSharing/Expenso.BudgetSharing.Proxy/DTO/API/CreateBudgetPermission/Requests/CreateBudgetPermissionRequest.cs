namespace Expenso.BudgetSharing.Proxy.DTO.API.CreateBudgetPermission.Requests;

public sealed record CreateBudgetPermissionRequest(Guid? BudgetPermissionId, Guid BudgetId, Guid OwnerId);