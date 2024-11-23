using Expenso.BudgetSharing.Domain.BudgetPermissionRequests.Repositories;
using Expenso.BudgetSharing.Domain.BudgetPermissionRequests.Rules;
using Expenso.BudgetSharing.Domain.BudgetPermissionRequests.Services.Interfaces;
using Expenso.BudgetSharing.Domain.BudgetPermissionRequests.ValueObjects;
using Expenso.BudgetSharing.Domain.BudgetPermissions;
using Expenso.BudgetSharing.Domain.BudgetPermissions.Repositories;
using Expenso.Shared.Domain.Types.Model;
using Expenso.Shared.Domain.Types.Rules;
using Expenso.Shared.Domain.Types.ValueObjects;
using Expenso.Shared.System.Types.Clock;
using Expenso.Shared.System.Types.Exceptions;
using Expenso.Shared.System.Types.Exceptions.Models;
using Expenso.UserPreferences.Shared;
using Expenso.UserPreferences.Shared.DTO.API.GetPreference.Request;
using Expenso.UserPreferences.Shared.DTO.API.GetPreference.Response;

namespace Expenso.BudgetSharing.Domain.BudgetPermissionRequests.Services;

internal sealed class ConfirmParticipantionDomainService : IConfirmParticipantionDomainService
{
    private readonly IBudgetPermissionRepository _budgetPermissionRepository;
    private readonly IBudgetPermissionRequestRepository _budgetPermissionRequestRepository;
    private readonly IClock _clock;
    private readonly IUserPreferencesProxy _userPreferencesProxy;

    public ConfirmParticipantionDomainService(IBudgetPermissionRequestRepository budgetPermissionRequestRepository,
        IBudgetPermissionRepository budgetPermissionRepository, IUserPreferencesProxy userPreferencesProxy,
        IClock clock)
    {
        _budgetPermissionRepository = budgetPermissionRepository ??
                                      throw new ArgumentNullException(paramName: nameof(budgetPermissionRepository));

        _budgetPermissionRequestRepository = budgetPermissionRequestRepository ??
                                             throw new ArgumentNullException(
                                                 paramName: nameof(budgetPermissionRequestRepository));

        _userPreferencesProxy = userPreferencesProxy ??
                                throw new ArgumentNullException(paramName: nameof(userPreferencesProxy));

        _clock = clock ?? throw new ArgumentNullException(paramName: nameof(clock));
    }

    public async Task ConfirmParticipantAsync(Guid? budgetPermissionRequestId, CancellationToken cancellationToken)
    {
        BudgetPermissionRequest? permissionRequest = await _budgetPermissionRequestRepository.GetByIdAsync(
            permissionId: BudgetPermissionRequestId.New(value: budgetPermissionRequestId),
            cancellationToken: cancellationToken);

        if (permissionRequest is null)
        {
            throw new NotFoundException(resourceName: nameof(BudgetPermissionRequest),
                identifierType: IdentifierType.PrimaryId(), identifier: budgetPermissionRequestId);
        }

        BudgetPermission? budgetPermission =
            await _budgetPermissionRepository.GetByBudgetIdAsync(budgetId: permissionRequest.BudgetId,
                cancellationToken: cancellationToken);

        if (budgetPermission is null)
        {
            throw new NotFoundException(resourceName: nameof(BudgetPermission),
                identifierType: IdentifierType.Custom(value: "Budget ID"), identifier: permissionRequest.BudgetId);
        }

        GetPreferencesResponse? preference = await _userPreferencesProxy.GetPreferences(
            getPreferenceRequest: new GetPreferencesRequest(UserId: budgetPermission.OwnerId.Value,
                PreferenceType: GetPreferencesRequestPreferenceTypes.Finance), cancellationToken: cancellationToken);

        if (preference?.FinancePreference is null)
        {
            throw new NotFoundException(resourceName: nameof(preference.FinancePreference),
                identifierType: IdentifierType.Custom(value: "User ID"), identifier: budgetPermission.OwnerId);
        }

        DomainModelState.CheckBusinessRules(businessRules:
        [
            new BusinessRuleCheck(
                BusinessRule: new PermissionCanBeAssignedOnlyToBudgetThatOwnerHasAllowedToAssigningPermissions(
                    budgetId: budgetPermission.BudgetId, ownerId: budgetPermission.OwnerId,
                    permissionTypeFromRequest: permissionRequest.PermissionType,
                    preferenceResponseFinancePreference: preference.FinancePreference,
                    currentPermissions: budgetPermission.Permissions.ToList().AsReadOnly()))
        ]);

        permissionRequest.Confirm(confirmationDate: DateAndTime.New(value: _clock.UtcNow));

        budgetPermission.AddPermission(participantId: permissionRequest.ParticipantId,
            permissionType: permissionRequest.PermissionType);

        await _budgetPermissionRequestRepository.UpdateAsync(permission: permissionRequest,
            cancellationToken: cancellationToken);

        await _budgetPermissionRepository.UpdateAsync(budgetPermission: budgetPermission,
            cancellationToken: cancellationToken);
    }
}