namespace Expenso.BudgetSharing.Shared.DTO.API.BudgetPermissions.GetBudgetPermissions.Request;

public sealed record GetBudgetPermissionsRequest(
    Guid? BudgetId = null,
    string? BudgetCode = null,
    Guid? OwnerId = null,
    Guid? ParticipantId = null,
    bool? ForCurrentUser = null,
    GetBudgetPermissionsRequestPermissionType PermissionType = GetBudgetPermissionsRequestPermissionType.All);