namespace Expenso.BudgetSharing.Domain.BudgetPermissionRequests.Services.Interfaces;

public interface IBudgetPermissionRequestExpirationDomainService
{
    Task MarkBudgetPermissionRequestAsExpireAsync(Guid? budgetPermissionRequestId, CancellationToken cancellationToken);
}