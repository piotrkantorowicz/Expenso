namespace Expenso.BudgetSharing.Domain.BudgetPermissionRequests.Services.Interfaces;

public interface IConfirmParticipationDomainService
{
    Task ConfirmParticipationAsync(Guid budgetPermissionId, CancellationToken cancellationToken);
}