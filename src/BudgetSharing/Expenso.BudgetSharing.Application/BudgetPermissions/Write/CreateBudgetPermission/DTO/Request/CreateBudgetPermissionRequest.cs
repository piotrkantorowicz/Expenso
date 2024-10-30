namespace Expenso.BudgetSharing.Application.BudgetPermissions.Write.CreateBudgetPermission.DTO.Request;

public sealed record CreateBudgetPermissionRequest(Guid? BudgetPermissionId, Guid BudgetId, Guid OwnerId);