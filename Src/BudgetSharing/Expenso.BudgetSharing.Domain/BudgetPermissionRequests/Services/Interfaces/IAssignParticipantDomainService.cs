using Expenso.BudgetSharing.Domain.Shared.Model.ValueObjects;

namespace Expenso.BudgetSharing.Domain.BudgetPermissionRequests.Services.Interfaces;

public interface IAssignParticipantDomainService
{
    Task<BudgetPermissionRequest> AssignParticipantAsync(Guid budgetId, string email,
        PermissionType permissionType, int expirationDays, CancellationToken cancellationToken);
}