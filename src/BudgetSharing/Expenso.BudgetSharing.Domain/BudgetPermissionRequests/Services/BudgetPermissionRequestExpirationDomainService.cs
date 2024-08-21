using Expenso.BudgetSharing.Domain.BudgetPermissionRequests.Repositories;
using Expenso.BudgetSharing.Domain.BudgetPermissionRequests.Services.Interfaces;
using Expenso.BudgetSharing.Domain.BudgetPermissionRequests.ValueObjects;
using Expenso.Shared.System.Types.Exceptions;

namespace Expenso.BudgetSharing.Domain.BudgetPermissionRequests.Services;

internal sealed class BudgetPermissionRequestExpirationDomainService : IBudgetPermissionRequestExpirationDomainService
{
    private readonly IBudgetPermissionRequestRepository _budgetPermissionRequestRepository;

    public BudgetPermissionRequestExpirationDomainService(
        IBudgetPermissionRequestRepository budgetPermissionRequestRepository)
    {
        _budgetPermissionRequestRepository = budgetPermissionRequestRepository ??
                                             throw new ArgumentNullException(
                                                 paramName: nameof(budgetPermissionRequestRepository));
    }

    public async Task MarkBudgetPermissionRequestAsExpire(Guid budgetPermissionRequestId,
        CancellationToken cancellationToken)
    {
        BudgetPermissionRequest? budgetPermissionRequest =
            await _budgetPermissionRequestRepository.GetByIdAsync(
                permissionId: BudgetPermissionRequestId.New(value: budgetPermissionRequestId),
                cancellationToken: cancellationToken);

        if (budgetPermissionRequest is null)
        {
            throw new NotFoundException(
                message: $"Budget permission request with id {budgetPermissionRequestId} hasn't been found");
        }

        budgetPermissionRequest.Expire();

        await _budgetPermissionRequestRepository.UpdateAsync(permission: budgetPermissionRequest,
            cancellationToken: cancellationToken);
    }
}