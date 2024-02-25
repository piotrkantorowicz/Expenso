namespace Expenso.BudgetSharing.Application.BudgetPermissions.Read.Internal.GetBudgetPermission.DTO.Responses;

public record GetBudgetPermissionResponse(
    Guid Id,
    Guid BudgetId,
    Guid OwnerId,
    ICollection<GetBudgetPermissionResponsePermission> Permissions);