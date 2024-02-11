using Expenso.BudgetSharing.Domain.BudgetPermissionRequests.ValueObjects;
using Expenso.BudgetSharing.Domain.Shared.Model.ValueObjects;

namespace Expenso.BudgetSharing.Domain.BudgetPermissionRequests.Services;

public interface IAssignParticipantDomainService
{
    Task<BudgetPermissionRequestId> AssignParticipantAsync(Guid budgetPermissionId, Guid participantId,
        PermissionType permissionType, int expirationDays, CancellationToken cancellationToken);
}