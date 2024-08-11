using Expenso.BudgetSharing.Domain.Shared.ValueObjects;
using Expenso.Shared.Domain.Types.Rules;

namespace Expenso.BudgetSharing.Domain.BudgetPermissions.Rules;

internal sealed class OwnerPermissionCannotBeRemoved(BudgetId budgetId, PermissionType? permissionType) : IBusinessRule
{
    private readonly BudgetId _budgetId = budgetId ?? throw new ArgumentNullException(paramName: nameof(budgetId));

    public string Message => $"Owner permission cannot be removed from budget {budgetId}.";

    public bool IsBroken()
    {
        return permissionType is null || permissionType == PermissionType.Owner;
    }
}