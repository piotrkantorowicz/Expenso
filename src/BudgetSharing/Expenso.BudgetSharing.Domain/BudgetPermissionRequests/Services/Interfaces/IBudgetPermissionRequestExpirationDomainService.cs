namespace Expenso.BudgetSharing.Domain.BudgetPermissionRequests.Services.Interfaces;

public interface IBudgetPermissionRequestExpirationDomainService
{
    Task MarkBudgetPermissionRequestAsExpire(Guid budgetPermissionRequestId, CancellationToken cancellationToken);
}