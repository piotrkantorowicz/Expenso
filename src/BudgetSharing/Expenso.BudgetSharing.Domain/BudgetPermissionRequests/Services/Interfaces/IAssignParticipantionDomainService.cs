using Expenso.BudgetSharing.Domain.Shared.ValueObjects;

namespace Expenso.BudgetSharing.Domain.BudgetPermissionRequests.Services.Interfaces;

public interface IAssignParticipantionDomainService
{
    Task<BudgetPermissionRequest> AssignParticipantAsync(BudgetId budgetId, string? email,
        PermissionType? permissionType, int expirationDays, CancellationToken cancellationToken);
}