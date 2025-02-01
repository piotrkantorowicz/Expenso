namespace Expenso.BudgetSharing.Shared.DTO.API.BudgetPermissions.AddPermission.Request;

public sealed record AddPermissionRequest(
    Guid BudgetPermissionId,
    Guid ParticipantId,
    AddPermissionRequestPermissionType PermissionType);