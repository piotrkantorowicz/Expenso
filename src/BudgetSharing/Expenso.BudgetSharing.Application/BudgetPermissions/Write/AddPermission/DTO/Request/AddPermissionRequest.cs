namespace Expenso.BudgetSharing.Application.BudgetPermissions.Write.AddPermission.DTO.Request;

public sealed record AddPermissionRequest(
    Guid BudgetPermissionId,
    Guid ParticipantId,
    AddPermissionRequestPermissionType PermissionType);