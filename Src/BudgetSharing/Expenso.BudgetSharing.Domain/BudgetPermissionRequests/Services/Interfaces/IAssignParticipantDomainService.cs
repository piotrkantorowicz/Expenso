using Expenso.BudgetSharing.Domain.BudgetPermissionRequests.ValueObjects;
using Expenso.BudgetSharing.Domain.Shared.Model.ValueObjects;

namespace Expenso.BudgetSharing.Domain.BudgetPermissionRequests.Services.Interfaces;

public interface IAssignParticipantDomainService
{
    Task<BudgetPermissionRequestId> AssignParticipantAsync(Guid budgetId, Guid participantId,
        PermissionType permissionType, int expirationDays, CancellationToken cancellationToken);
}