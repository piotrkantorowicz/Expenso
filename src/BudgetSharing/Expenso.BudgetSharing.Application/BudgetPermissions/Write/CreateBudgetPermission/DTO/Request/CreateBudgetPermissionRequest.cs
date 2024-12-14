namespace Expenso.BudgetSharing.Application.BudgetPermissions.Write.CreateBudgetPermission.DTO.Request;

public sealed record CreateBudgetPermissionRequest(
    Guid BudgetId,
    string BudgetCode,
    Guid OwnerId,
    Guid? BudgetPermissionId = null);