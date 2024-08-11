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
                                                                                   paramName: nameof(
                                                                                       budgetPermissionRepository));

    private readonly IBudgetPermissionRequestRepository _budgetPermissionRequestRepository =
        budgetPermissionRequestRepository ??
        throw new ArgumentNullException(paramName: nameof(budgetPermissionRequestRepository));

    private readonly IUserPreferencesProxy _userPreferencesProxy =
        userPreferencesProxy ?? throw new ArgumentNullException(paramName: nameof(userPreferencesProxy));

    public async Task ConfirmParticipationAsync(Guid budgetPermissionRequestId, CancellationToken cancellationToken)
    {
        BudgetPermissionRequest? permissionRequest = await _budgetPermissionRequestRepository.GetByIdAsync(
            permissionId: BudgetPermissionRequestId.New(value: budgetPermissionRequestId),
            cancellationToken: cancellationToken);

        if (permissionRequest is null)
        {
            throw new NotFoundException(
                message: $"Budget permission request with id {budgetPermissionRequestId} hasn't been found.");
        }

        BudgetPermission? budgetPermission =
            await _budgetPermissionRepository.GetByBudgetIdAsync(budgetId: permissionRequest.BudgetId,
                cancellationToken: cancellationToken);

        if (budgetPermission is null)
        {
            throw new NotFoundException(
                message: $"Budget permission with id {permissionRequest.BudgetId} hasn't been found.");
        }

        GetPreferenceResponse? preference = await _userPreferencesProxy.GetUserPreferencesAsync(
            userId: budgetPermission.OwnerId.Value, includeFinancePreferences: true,
            includeNotificationPreferences: false, includeGeneralPreferences: false,
            cancellationToken: cancellationToken);

        if (preference?.FinancePreference is null)
        {
            throw new NotFoundException(
                message: $"Finance preferences for user {budgetPermission.OwnerId} haven't been found.");
        }

        DomainModelState.CheckBusinessRules(businessRules:
        [
            (new PermissionCanBeAssignedOnlyToBudgetThatOwnerHasAllowedToAssigningPermissions(budgetId: budgetPermission.BudgetId, ownerId: budgetPermission.OwnerId, permissionTypeFromRequest: permissionRequest.PermissionType, preferenceResponseFinancePreference: preference.FinancePreference, currentPermissions: budgetPermission.Permissions.ToList().AsReadOnly()),
                false)
        ]);

        permissionRequest.Confirm();

        budgetPermission.AddPermission(participantId: permissionRequest.ParticipantId,
            permissionType: permissionRequest.PermissionType);

        await _budgetPermissionRequestRepository.UpdateAsync(permission: permissionRequest,
            cancellationToken: cancellationToken);

        await _budgetPermissionRepository.UpdateAsync(budgetPermission: budgetPermission,
            cancellationToken: cancellationToken);
    }
}