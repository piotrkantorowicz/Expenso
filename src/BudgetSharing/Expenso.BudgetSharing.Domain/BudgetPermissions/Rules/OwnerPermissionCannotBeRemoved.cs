using Expenso.BudgetSharing.Domain.Shared.ValueObjects;
using Expenso.Shared.Domain.Types.Rules;

namespace Expenso.BudgetSharing.Domain.BudgetPermissions.Rules;

internal sealed class OwnerPermissionCannotBeRemoved : IBusinessRule
{
    private readonly BudgetId _budgetId;
    private readonly PermissionType? _permissionType;

    public OwnerPermissionCannotBeRemoved(BudgetId budgetId, PermissionType? permissionType)
    {
        _budgetId = budgetId ?? throw new ArgumentNullException(paramName: nameof(budgetId));
        _permissionType = permissionType;
    }

    public string Message => $"Owner permission cannot be removed from budget {_budgetId}.";

    public bool IsBroken()
    {
        return _permissionType is null || _permissionType == PermissionType.Owner;
    }
}