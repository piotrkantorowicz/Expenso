using Expenso.BudgetSharing.Domain.BudgetPermissions;
using Expenso.BudgetSharing.Domain.Shared.Model.ValueObjects;
using Expenso.Shared.Domain.Types.Rules;
using Expenso.UserPreferences.Proxy.DTO.API.GetPreference.Response;

namespace Expenso.BudgetSharing.Domain.BudgetPermissionRequests.Rules;

internal sealed class PermissionCanBeAssignedOnlyToBudgetThatOwnerHasAllowedToAssigningPermissions(
    BudgetId budgetId,
    PersonId ownerId,
    PermissionType permissionTypeFromRequest,
    GetPreferenceResponse_FinancePreference preferenceResponseFinancePreference,
    IReadOnlyCollection<Permission> currentPermissions) : IBusinessRule
{
    private readonly BudgetId _budgetId = budgetId ?? throw new ArgumentNullException(nameof(budgetId));

    private readonly IReadOnlyCollection<Permission> _currentPermissions =
        currentPermissions ?? throw new ArgumentNullException(nameof(currentPermissions));

    private readonly GetPreferenceResponse_FinancePreference
        _preferenceExternalPreferenceResponseFinancePreferenceExternal = preferenceResponseFinancePreference ??
                                                                         throw new ArgumentNullException(
                                                                             nameof(
                                                                                 preferenceResponseFinancePreference));

    private readonly PersonId _ownerId = ownerId ?? throw new ArgumentNullException(nameof(ownerId));

    private readonly PermissionType _permissionTypeFromRequest = permissionTypeFromRequest ??
                                                                 throw new ArgumentNullException(
                                                                     nameof(permissionTypeFromRequest));

    public string Message => $"Permission of type {_permissionTypeFromRequest} can't be assigned to budget with id " +
                             $"{_budgetId}, because permission type is not valid or budget owner with id: " +
                             $"{_ownerId} don't allow any or more participants.";

    public bool IsBroken()
    {
        bool isPermissionTypeFromRequestValid = _permissionTypeFromRequest == PermissionType.SubOwner ||
                                                _permissionTypeFromRequest == PermissionType.Reviewer;

        if (!isPermissionTypeFromRequestValid)
        {
            return true;
        }

        int permissionsCountForTypeFromRequest =
            _currentPermissions.Count(x => x.PermissionType == _permissionTypeFromRequest);

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