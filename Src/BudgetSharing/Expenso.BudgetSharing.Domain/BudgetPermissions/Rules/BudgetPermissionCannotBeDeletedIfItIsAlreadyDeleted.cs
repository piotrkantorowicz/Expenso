using Expenso.Shared.Domain.Types.Rules;
using Expenso.Shared.Domain.Types.ValueObjects;

namespace Expenso.BudgetSharing.Domain.BudgetPermissions.Rules;

internal sealed class BudgetPermissionCannotBeDeletedIfItIsAlreadyDeleted(SafeDeletion? removalInfo) : IBusinessRule
{
    public string Message => "Budget permission cannot be removed if it is already removed";

    public bool IsBroken()
    {
        return removalInfo?.IsDeleted == true;
    }
}