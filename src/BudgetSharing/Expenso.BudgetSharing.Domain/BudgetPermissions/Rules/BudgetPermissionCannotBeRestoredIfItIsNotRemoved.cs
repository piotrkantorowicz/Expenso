using Expenso.BudgetSharing.Domain.BudgetPermissions.ValueObjects;
using Expenso.Shared.Domain.Types.Rules;
using Expenso.Shared.Domain.Types.ValueObjects;

namespace Expenso.BudgetSharing.Domain.BudgetPermissions.Rules;

internal sealed class BudgetPermissionCannotBeRestoredIfItIsNotRemoved : IBusinessRule
{
    private readonly BudgetPermissionId _budgetPermissionId;
    private readonly SafeDeletion? _removalInfo;

    public BudgetPermissionCannotBeRestoredIfItIsNotRemoved(BudgetPermissionId budgetPermissionId,
        SafeDeletion? removalInfo)
    {
        _removalInfo = removalInfo;

        _budgetPermissionId =
            budgetPermissionId ?? throw new ArgumentNullException(paramName: nameof(budgetPermissionId));
    }

    public string Message => $"Budget permission with id: {_budgetPermissionId} is not deleted";

    public bool IsBroken()
    {
        return _removalInfo?.IsDeleted is not true;
    }
}