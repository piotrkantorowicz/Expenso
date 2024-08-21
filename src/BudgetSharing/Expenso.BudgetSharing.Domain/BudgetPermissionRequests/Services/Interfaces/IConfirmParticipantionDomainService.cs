namespace Expenso.BudgetSharing.Domain.BudgetPermissionRequests.Services.Interfaces;

public interface IConfirmParticipantionDomainService
{
    Task ConfirmParticipantAsync(Guid budgetPermissionRequestId, CancellationToken cancellationToken);
}