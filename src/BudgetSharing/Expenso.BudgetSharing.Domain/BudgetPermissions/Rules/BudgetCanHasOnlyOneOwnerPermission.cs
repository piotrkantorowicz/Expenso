using Expenso.BudgetSharing.Domain.Shared.ValueObjects;
using Expenso.Shared.Domain.Types.Rules;

namespace Expenso.BudgetSharing.Domain.BudgetPermissions.Rules;

internal sealed class BudgetCanHasOnlyOneOwnerPermission(
    BudgetId budgetId,
    PermissionType permissionType,
    IEnumerable<Permission> permissions) : IBusinessRule
{
    private readonly BudgetId _budgetId = budgetId ?? throw new ArgumentNullException(paramName: nameof(budgetId));

    private readonly IEnumerable<Permission> _permissions =
        permissions ?? throw new ArgumentNullException(paramName: nameof(permissions));

    private readonly PermissionType _permissionType =
        permissionType ?? throw new ArgumentNullException(paramName: nameof(permissionType));

    public string Message => $"Budget {_budgetId} can have only one owner permission";

    public bool IsBroken()
    {
        return _permissions.Any(predicate: p => p.PermissionType == PermissionType.Owner) &&
               _permissionType == PermissionType.Owner;
    }
}