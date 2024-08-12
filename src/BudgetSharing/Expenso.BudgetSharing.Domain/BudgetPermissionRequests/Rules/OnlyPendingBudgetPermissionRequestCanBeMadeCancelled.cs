using Expenso.BudgetSharing.Domain.BudgetPermissionRequests.ValueObjects;
using Expenso.Shared.Domain.Types.Rules;

namespace Expenso.BudgetSharing.Domain.BudgetPermissionRequests.Rules;

internal sealed class OnlyPendingBudgetPermissionRequestCanBeMadeCancelled(
    BudgetPermissionRequestId budgetPermissionRequestId,
    BudgetPermissionRequestStatus status) : IBusinessRule
{
    private readonly BudgetPermissionRequestId _budgetPermissionRequestId = budgetPermissionRequestId ??
                                                                            throw new ArgumentNullException(
                                                                                paramName: nameof(
                                                                                    budgetPermissionRequestId));

    private readonly BudgetPermissionRequestStatus _status =
        status ?? throw new ArgumentNullException(paramName: nameof(status));

    public string Message =>
        $"Only pending budget permission request {_budgetPermissionRequestId} can be made cancelled";

    public bool IsBroken()
    {
        return !_status.IsPending();
    }
}