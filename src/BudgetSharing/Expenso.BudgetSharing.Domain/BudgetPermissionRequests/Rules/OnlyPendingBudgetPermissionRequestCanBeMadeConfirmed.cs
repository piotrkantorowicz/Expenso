using Expenso.BudgetSharing.Domain.BudgetPermissionRequests.ValueObjects;
using Expenso.Shared.Domain.Types.Rules;

namespace Expenso.BudgetSharing.Domain.BudgetPermissionRequests.Rules;

internal sealed class OnlyPendingBudgetPermissionRequestCanBeMadeConfirmed : IBusinessRule
{
    private readonly BudgetPermissionRequestId _budgetPermissionRequestId;
    private readonly BudgetPermissionRequestStatus _status;

    public OnlyPendingBudgetPermissionRequestCanBeMadeConfirmed(BudgetPermissionRequestId budgetPermissionRequestId,
        BudgetPermissionRequestStatus status)
    {
        _budgetPermissionRequestId = budgetPermissionRequestId ??
                                     throw new ArgumentNullException(paramName: nameof(budgetPermissionRequestId));

        _status = status ?? throw new ArgumentNullException(paramName: nameof(status));
    }

    public string Message =>
        $"Only pending budget permission request {_budgetPermissionRequestId} can be made confirmed";

    public bool IsBroken()
    {
        return !_status.IsPending();
    }
}