using Expenso.BudgetSharing.Domain.BudgetPermissionRequests.Services.Interfaces;
using Expenso.BudgetSharing.Proxy.DTO.MessageBus.BudgetPermissionRequests;
using Expenso.Shared.Integration.Events;

namespace Expenso.BudgetSharing.Domain.BudgetPermissionRequests.EventHandlers.External;

internal sealed class
    BudgetPermissionRequestExpiredIntegrationEventHandler : IIntegrationEventHandler<
    BudgetPermissionRequestExpiredIntegrationEvent>
{
    private readonly IBudgetPermissionRequestExpirationDomainService _budgetPermissionRequestExpirationDomainService;

    public BudgetPermissionRequestExpiredIntegrationEventHandler(
        IBudgetPermissionRequestExpirationDomainService budgetPermissionRequestExpirationDomainService)
    {
        _budgetPermissionRequestExpirationDomainService = budgetPermissionRequestExpirationDomainService ??
                                                          throw new ArgumentNullException(
                                                              paramName: nameof(
                                                                  budgetPermissionRequestExpirationDomainService));
    }

    public async Task HandleAsync(BudgetPermissionRequestExpiredIntegrationEvent @event,
        CancellationToken cancellationToken)
    {
        await _budgetPermissionRequestExpirationDomainService.MarkBudgetPermissionRequestAsExpireAsync(
            budgetPermissionRequestId: @event.BudgetPermissionRequestId, cancellationToken: cancellationToken);
    }
}