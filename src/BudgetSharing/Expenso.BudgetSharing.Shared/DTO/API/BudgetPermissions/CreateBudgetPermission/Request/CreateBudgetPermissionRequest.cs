namespace Expenso.BudgetSharing.Shared.DTO.API.BudgetPermissions.CreateBudgetPermission.Request;

public sealed record CreateBudgetPermissionRequest(
    Guid BudgetId,
    string BudgetCode,
    Guid OwnerId,
    Guid? BudgetPermissionId = null);