using Expenso.BudgetSharing.Domain.BudgetPermissionRequests.Repositories;
using Expenso.BudgetSharing.Domain.BudgetPermissionRequests.Services.Interfaces;
using Expenso.BudgetSharing.Domain.BudgetPermissionRequests.ValueObjects;
using Expenso.Shared.System.Types.Exceptions;
using Expenso.Shared.System.Types.Exceptions.Models;

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

    public async Task MarkBudgetPermissionRequestAsExpireAsync(Guid? budgetPermissionRequestId,
        CancellationToken cancellationToken)
    {
        BudgetPermissionRequestId typedBudgetPermissionRequestId =
            BudgetPermissionRequestId.New(value: budgetPermissionRequestId);

        BudgetPermissionRequest? budgetPermissionRequest = await _budgetPermissionRequestRepository.GetByIdAsync(
            permissionId: typedBudgetPermissionRequestId,
                cancellationToken: cancellationToken);

        if (budgetPermissionRequest is null)
        {
            throw new NotFoundException(resourceName: nameof(BudgetPermissionRequest),
                identifierType: IdentifierType.PrimaryId(), identifier: typedBudgetPermissionRequestId);
        }

        budgetPermissionRequest.Expire();

        await _budgetPermissionRequestRepository.UpdateAsync(permission: budgetPermissionRequest,
            cancellationToken: cancellationToken);
    }
}