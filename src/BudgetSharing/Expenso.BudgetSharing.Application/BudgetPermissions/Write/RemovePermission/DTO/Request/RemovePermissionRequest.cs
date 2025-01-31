namespace Expenso.BudgetSharing.Application.BudgetPermissions.Write.RemovePermission.DTO.Request;

public sealed record RemovePermissionRequest(Guid BudgetPermissionId, Guid ParticipantId);