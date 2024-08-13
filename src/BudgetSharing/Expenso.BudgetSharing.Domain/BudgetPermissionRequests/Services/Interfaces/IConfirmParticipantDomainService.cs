namespace Expenso.BudgetSharing.Domain.BudgetPermissionRequests.Services.Interfaces;

public interface IConfirmParticipantDomainService
{
    Task ConfirmParticipantAsync(Guid budgetPermissionRequestId, CancellationToken cancellationToken);
}