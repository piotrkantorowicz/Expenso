using System.Text;

using Expenso.BudgetSharing.Domain.Shared.Model.ValueObjects;
using Expenso.Shared.Domain.Types.Rules;

namespace Expenso.BudgetSharing.Domain.BudgetPermissions.Model.Rules;

internal sealed class BudgetCanHasOnlyOneOwnerPermission(BudgetId budgetId, IEnumerable<Permission> permissions)
    : IBusinessRule
{
    private readonly BudgetId _budgetId = budgetId ?? throw new ArgumentNullException(nameof(budgetId));

    private readonly IEnumerable<Permission> _permissions =
        permissions ?? throw new ArgumentNullException(nameof(permissions));

    public string Message => new StringBuilder()
        .Append("Budget ")
        .Append(_budgetId)
        .Append(" can have only one owner permission")
        .ToString();

    public bool IsBroken()
    {
        return _permissions.Count(p => p.PermissionType == PermissionType.Owner) > 1;
    }
}