namespace Expenso.BudgetSharing.Domain.BudgetPermissionRequests.Services.Interfaces;

public interface IBudgetPermissionRequestExpireDomainService
{
    Task MarkBudgetPermissionRequestAsExpire(Guid budgetPermissionRequestId, CancellationToken cancellationToken);
}