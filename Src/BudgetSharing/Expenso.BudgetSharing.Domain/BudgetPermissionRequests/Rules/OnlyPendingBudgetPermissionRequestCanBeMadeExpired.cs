using Expenso.BudgetSharing.Domain.BudgetPermissionRequests.ValueObjects;
using Expenso.Shared.Domain.Types.Rules;

namespace Expenso.BudgetSharing.Domain.BudgetPermissionRequests.Rules;

public class OnlyPendingBudgetPermissionRequestCanBeMadeExpired(
    BudgetPermissionRequestId budgetPermissionRequestId,
    BudgetPermissionRequestStatus status) : IBusinessRule
{
    private readonly BudgetPermissionRequestId _budgetPermissionRequestId =
        budgetPermissionRequestId ?? throw new ArgumentNullException(nameof(budgetPermissionRequestId));

    private readonly BudgetPermissionRequestStatus _status = status ?? throw new ArgumentNullException(nameof(status));

    public string Message =>
        $"Only pending budget permission request {_budgetPermissionRequestId} can be made expired.";

    public bool IsBroken()
    {
        return !_status.IsPending();
    }
}