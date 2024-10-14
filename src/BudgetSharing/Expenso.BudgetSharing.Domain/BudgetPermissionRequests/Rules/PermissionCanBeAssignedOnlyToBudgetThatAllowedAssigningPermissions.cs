using Expenso.BudgetSharing.Domain.BudgetPermissions;
using Expenso.BudgetSharing.Domain.Shared.ValueObjects;
using Expenso.Shared.Domain.Types.Rules;
using Expenso.UserPreferences.Shared.DTO.API.GetPreference.Response;

namespace Expenso.BudgetSharing.Domain.BudgetPermissionRequests.Rules;

internal sealed class PermissionCanBeAssignedOnlyToBudgetThatOwnerHasAllowedToAssigningPermissions : IBusinessRule
{
    private readonly BudgetId _budgetId;
    private readonly IReadOnlyCollection<Permission> _currentPermissions;
    private readonly PersonId _ownerId;
    private readonly PermissionType _permissionTypeFromRequest;

    private readonly GetPreferencesResponse_FinancePreference
        _preferenceExternalPreferenceResponseFinancePreferenceExternal;

    public PermissionCanBeAssignedOnlyToBudgetThatOwnerHasAllowedToAssigningPermissions(BudgetId budgetId,
        PersonId ownerId, PermissionType permissionTypeFromRequest,
        GetPreferencesResponse_FinancePreference preferenceResponseFinancePreference,
        IReadOnlyCollection<Permission> currentPermissions)
    {
        _budgetId = budgetId ?? throw new ArgumentNullException(paramName: nameof(budgetId));

        _currentPermissions =
            currentPermissions ?? throw new ArgumentNullException(paramName: nameof(currentPermissions));

        _ownerId = ownerId ?? throw new ArgumentNullException(paramName: nameof(ownerId));

        _permissionTypeFromRequest = permissionTypeFromRequest ??
                                     throw new ArgumentNullException(paramName: nameof(permissionTypeFromRequest));

        _preferenceExternalPreferenceResponseFinancePreferenceExternal = preferenceResponseFinancePreference ??
                                                                         throw new ArgumentNullException(
                                                                             paramName: nameof(
                                                                                 preferenceResponseFinancePreference));
    }

    public string Message =>
        $"Permission of type {_permissionTypeFromRequest} can't be assigned to budget with id {_budgetId}, because permission type is not valid or budget owner with id: {_ownerId} don't allow any or more participants.";

    public bool IsBroken()
    {
        bool isPermissionTypeFromRequestValid = _permissionTypeFromRequest == PermissionType.SubOwner ||
                                                _permissionTypeFromRequest == PermissionType.Reviewer;

        if (!isPermissionTypeFromRequestValid)
        {
            return true;
        }

        int permissionsCountForTypeFromRequest =
            _currentPermissions.Count(predicate: x => x.PermissionType == _permissionTypeFromRequest);

        if (_permissionTypeFromRequest == PermissionType.SubOwner)
        {
            return !_preferenceExternalPreferenceResponseFinancePreferenceExternal.AllowAddFinancePlanSubOwners ||
                   _preferenceExternalPreferenceResponseFinancePreferenceExternal.MaxNumberOfSubFinancePlanSubOwners <=
                   permissionsCountForTypeFromRequest;
        }

        if (_permissionTypeFromRequest == PermissionType.Reviewer)
        {
            return !_preferenceExternalPreferenceResponseFinancePreferenceExternal.AllowAddFinancePlanReviewers ||
                   _preferenceExternalPreferenceResponseFinancePreferenceExternal.MaxNumberOfFinancePlanReviewers <=
                   permissionsCountForTypeFromRequest;
        }

        return false;
    }
}