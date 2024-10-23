namespace Expenso.BudgetSharing.Application.BudgetPermissions.Write.RemovePermission.DTO;

public sealed record RemovePermissionRequest(Guid BudgetPermissionId, Guid ParticipantId);