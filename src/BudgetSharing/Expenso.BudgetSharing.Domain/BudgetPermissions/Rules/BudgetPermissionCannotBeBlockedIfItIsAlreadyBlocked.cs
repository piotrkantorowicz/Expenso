using Expenso.BudgetSharing.Domain.BudgetPermissions.ValueObjects;
using Expenso.Shared.Domain.Types.Rules;
using Expenso.Shared.Domain.Types.ValueObjects;

namespace Expenso.BudgetSharing.Domain.BudgetPermissions.Rules;

internal sealed class BudgetPermissionCannotBeBlockedIfItIsAlreadyBlocked : IBusinessRule
{
    private readonly Blocker? _blockInfo;
    private readonly BudgetPermissionId _budgetPermissionId;

    public BudgetPermissionCannotBeBlockedIfItIsAlreadyBlocked(BudgetPermissionId budgetPermissionId,
        Blocker? blockInfo)
    {
        _blockInfo = blockInfo;

        _budgetPermissionId =
            budgetPermissionId ?? throw new ArgumentNullException(paramName: nameof(budgetPermissionId));
    }

    public string Message => $"Budget permission with id: {_budgetPermissionId} is already deleted";

    public bool IsBroken()
    {
        return _blockInfo?.IsBlocked is true;
    }
}