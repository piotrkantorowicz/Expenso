using System.Text;

using Expenso.BudgetSharing.Domain.BudgetPermissions;
using Expenso.BudgetSharing.Domain.Shared.ValueObjects;
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
    private readonly BudgetId _budgetId = budgetId ?? throw new ArgumentNullException(paramName: nameof(budgetId));

    private readonly IReadOnlyCollection<Permission> _currentPermissions =
        currentPermissions ?? throw new ArgumentNullException(paramName: nameof(currentPermissions));

    private readonly PersonId _ownerId = ownerId ?? throw new ArgumentNullException(paramName: nameof(ownerId));

    private readonly PermissionType _permissionTypeFromRequest = permissionTypeFromRequest ??
                                                                 throw new ArgumentNullException(
                                                                     paramName: nameof(permissionTypeFromRequest));

    private readonly GetPreferenceResponse_FinancePreference
        _preferenceExternalPreferenceResponseFinancePreferenceExternal = preferenceResponseFinancePreference ??
                                                                         throw new ArgumentNullException(
                                                                             paramName: nameof(
                                                                                 preferenceResponseFinancePreference));

    public string Message => new StringBuilder()
        .Append(value: "Permission of type ")
        .Append(value: _permissionTypeFromRequest)
        .Append(value: " can't be assigned to budget with id ")
        .Append(value: _budgetId)
        .Append(value: ", because permission type is not valid or budget owner with id: ")
        .Append(value: _ownerId)
        .Append(value: " don't allow any or more participants.")
        .ToString();

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