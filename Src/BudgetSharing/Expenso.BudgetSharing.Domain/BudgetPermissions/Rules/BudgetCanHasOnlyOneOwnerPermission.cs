using Expenso.BudgetSharing.Domain.Shared.Model.ValueObjects;
using Expenso.Shared.Domain.Types.Rules;

namespace Expenso.BudgetSharing.Domain.BudgetPermissions.Rules;

internal sealed class BudgetCanHasOnlyOneOwnerPermission(BudgetId budgetId, IEnumerable<Permission> permissions)
    : IBusinessRule
{
    private readonly BudgetId _budgetId = budgetId ?? throw new ArgumentNullException(nameof(budgetId));

    private readonly IEnumerable<Permission> _permissions =
        permissions ?? throw new ArgumentNullException(nameof(permissions));

    public string Message => $"Budget {_budgetId} can have only one owner permission.";

    public bool IsBroken()
    {
        return _permissions.Count(p => p.PermissionType == PermissionType.Owner) > 1;
    }
}