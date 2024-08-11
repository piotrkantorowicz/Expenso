using Expenso.BudgetSharing.Domain.BudgetPermissions.ValueObjects;
using Expenso.Shared.Domain.Types.Rules;
using Expenso.Shared.Domain.Types.ValueObjects;

namespace Expenso.BudgetSharing.Domain.BudgetPermissions.Rules;

internal sealed class BudgetPermissionCannotBeDeletedIfItIsAlreadyDeleted(
    BudgetPermissionId budgetPermissionId,
    SafeDeletion? removalInfo) : IBusinessRule
{
    private readonly BudgetPermissionId _budgetPermissionId =
        budgetPermissionId ?? throw new ArgumentNullException(paramName: nameof(budgetPermissionId));

    public string Message => $"Budget permission with id: {_budgetPermissionId} is already deleted.";

    public bool IsBroken()
    {
        return removalInfo?.IsDeleted == true;
    }
}