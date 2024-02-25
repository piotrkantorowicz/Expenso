using Expenso.Shared.Domain.Types.Rules;
using Expenso.Shared.Domain.Types.ValueObjects;

namespace Expenso.BudgetSharing.Domain.BudgetPermissions.Rules;

public class BudgetPermissionCannotBeRestoredIfItIsNotRemoved(SafeDeletion? removalInfo) : IBusinessRule
{
    public string Message => "Budget permission cannot be restored if it is not removed";

    public bool IsBroken()
    {
        return removalInfo?.IsDeleted == false;
    }
}