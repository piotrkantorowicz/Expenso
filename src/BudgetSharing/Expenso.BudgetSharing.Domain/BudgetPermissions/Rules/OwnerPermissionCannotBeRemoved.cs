using Expenso.BudgetSharing.Domain.Shared.ValueObjects;
using Expenso.Shared.Domain.Types.Rules;

namespace Expenso.BudgetSharing.Domain.BudgetPermissions.Rules;

internal sealed class OwnerPermissionCannotBeRemoved : IBusinessRule
{
    private readonly BudgetId _budgetId;
    private readonly BudgetId _budgetId1;
    private readonly PermissionType? _permissionType;

    public OwnerPermissionCannotBeRemoved(BudgetId budgetId, PermissionType? permissionType)
    {
        _budgetId1 = budgetId;
        _permissionType = permissionType;
        _budgetId = budgetId ?? throw new ArgumentNullException(paramName: nameof(budgetId));
    }

    public string Message => $"Owner permission cannot be removed from budget {_budgetId1}";

    public bool IsBroken()
    {
        return _permissionType is null || _permissionType == PermissionType.Owner;
    }
}