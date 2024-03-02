using Expenso.BudgetSharing.Domain.BudgetPermissionRequests.Repositories;
using Expenso.BudgetSharing.Domain.BudgetPermissionRequests.Rules;
using Expenso.BudgetSharing.Domain.BudgetPermissionRequests.Services.Interfaces;
using Expenso.BudgetSharing.Domain.BudgetPermissionRequests.ValueObjects;
using Expenso.BudgetSharing.Domain.BudgetPermissions;
using Expenso.BudgetSharing.Domain.BudgetPermissions.Repositories;
using Expenso.Shared.Domain.Types.Model;
using Expenso.Shared.System.Types.Exceptions;
using Expenso.UserPreferences.Proxy;
using Expenso.UserPreferences.Proxy.DTO.API.GetPreference.Response;

namespace Expenso.BudgetSharing.Domain.BudgetPermissionRequests.Services;

internal sealed class ConfirmParticipationDomainService(
    IBudgetPermissionRequestRepository budgetPermissionRequestRepository,
    IBudgetPermissionRepository budgetPermissionRepository,
    IUserPreferencesProxy userPreferencesProxy) : IConfirmParticipationDomainService
{
    private readonly IBudgetPermissionRepository _budgetPermissionRepository = budgetPermissionRepository ??
                                                                               throw new ArgumentNullException(
                                                                                   nameof(budgetPermissionRepository));

    private readonly IBudgetPermissionRequestRepository _budgetPermissionRequestRepository =
        budgetPermissionRequestRepository ?? throw new ArgumentNullException(nameof(budgetPermissionRequestRepository));

    private readonly IUserPreferencesProxy _userPreferencesProxy =
        userPreferencesProxy ?? throw new ArgumentNullException(nameof(userPreferencesProxy));

    public async Task ConfirmParticipationAsync(Guid budgetPermissionRequestId, CancellationToken cancellationToken)
    {
        BudgetPermissionRequest? permissionRequest = await _budgetPermissionRequestRepository.GetByIdAsync(
            BudgetPermissionRequestId.New(budgetPermissionRequestId), cancellationToken);

        if (permissionRequest is null)
        {
            throw new NotFoundException(
                $"Budget permission request with id {budgetPermissionRequestId} hasn't been found.");
        }

        BudgetPermission? budgetPermission =
            await _budgetPermissionRepository.GetByBudgetIdAsync(permissionRequest.BudgetId, cancellationToken);

        if (budgetPermission is null)
        {
            throw new NotFoundException($"Budget permission with id {permissionRequest.BudgetId} hasn't been found.");
        }

        GetPreferenceResponse? preference = await _userPreferencesProxy.GetUserPreferencesAsync(
            budgetPermission.OwnerId.Value, true, false, false, cancellationToken);

        if (preference?.FinancePreference is null)
        {
            throw new NotFoundException($"Finance preferences for user {budgetPermission.OwnerId} haven't been found.");
        }

        DomainModelState.CheckBusinessRules([
            (new PermissionCanBeAssignedOnlyToBudgetThatOwnerHasAllowedToAssigningPermissions(budgetPermission.BudgetId, budgetPermission.OwnerId, permissionRequest.PermissionType, preference.FinancePreference, budgetPermission.Permissions.ToList().AsReadOnly()),
                false)
        ]);

        permissionRequest.Confirm();
        budgetPermission.AddPermission(permissionRequest.ParticipantId, permissionRequest.PermissionType);
        await _budgetPermissionRequestRepository.UpdateAsync(permissionRequest, cancellationToken);
        await _budgetPermissionRepository.UpdateAsync(budgetPermission, cancellationToken);
    }
}