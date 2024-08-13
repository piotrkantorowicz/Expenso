using Expenso.BudgetSharing.Domain.BudgetPermissionRequests.Services.Interfaces;
using Expenso.BudgetSharing.Proxy.DTO.MessageBus.BudgetPermissionRequests;
using Expenso.Shared.Integration.Events;

namespace Expenso.BudgetSharing.Domain.BudgetPermissionRequests.EventHandlers.External;

internal sealed class
    BudgetPermissionRequestExpiredIntegrationEventHandler : IIntegrationEventHandler<
    BudgetPermissionRequestExpiredIntegrationEvent>
{
    private readonly IBudgetPermissionRequestExpireDomainService _budgetPermissionRequestExpireDomainService;

    public BudgetPermissionRequestExpiredIntegrationEventHandler(
        IBudgetPermissionRequestExpireDomainService budgetPermissionRequestExpireDomainService)
    {
        _budgetPermissionRequestExpireDomainService = budgetPermissionRequestExpireDomainService ??
                                                      throw new ArgumentNullException(
                                                          paramName: nameof(
                                                              budgetPermissionRequestExpireDomainService));
    }

    public async Task HandleAsync(BudgetPermissionRequestExpiredIntegrationEvent @event,
        CancellationToken cancellationToken)
    {
        await _budgetPermissionRequestExpireDomainService.MarkBudgetPermissionRequestAsExpire(
            budgetPermissionRequestId: @event.BudgetPermissionRequestId, cancellationToken: cancellationToken);
    }
}