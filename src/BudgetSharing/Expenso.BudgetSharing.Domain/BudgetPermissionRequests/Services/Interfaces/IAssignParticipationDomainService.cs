using Expenso.BudgetSharing.Domain.BudgetPermissionRequests.ValueObjects;
using Expenso.BudgetSharing.Domain.Shared.ValueObjects;

namespace Expenso.BudgetSharing.Domain.BudgetPermissionRequests.Services.Interfaces;

public interface IAssignParticipationDomainService
{
    Task<BudgetPermissionRequest> AssignParticipantAsync(BudgetPermissionRequestId? budgetPermissionRequestId,
        BudgetId budgetId, string? email, PermissionType? permissionType, int expirationDays,
        CancellationToken cancellationToken);
}