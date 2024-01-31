using System.Text;

using Expenso.BudgetSharing.Domain.BudgetPermissions.Model.ValueObjects;
using Expenso.BudgetSharing.Domain.Shared.Model.ValueObjects;
using Expenso.Shared.Domain.Types.Rules;

namespace Expenso.BudgetSharing.Domain.BudgetPermissions.Model.Rules;

public class BudgetMustContainsPermissionForProvidedUsersAndTypes(
    BudgetId budgetId,
    PermissionId permissionId,
    Permission? permission) : IBusinessRule
{
    private readonly BudgetId _budgetId = budgetId ?? throw new ArgumentNullException(nameof(budgetId));
    private readonly PermissionId _permissionId = permissionId ?? throw new ArgumentNullException(nameof(permissionId));

    public string Message =>
        new StringBuilder()
            .Append("Budget ")
            .Append(_budgetId)
            .Append(" does not have permission with id: ")
            .Append(_permissionId)
            .ToString();

    public bool IsBroken()
    {
        return permission is null;
    }
}