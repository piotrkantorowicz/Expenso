namespace Expenso.BudgetSharing.Application.BudgetPermissions.Write.Internal.CreateBudgetPermission.DTO.Request;

public sealed record CreateBudgetPermissionRequest(Guid? BudgetPermissionId, Guid BudgetId, Guid OwnerId);