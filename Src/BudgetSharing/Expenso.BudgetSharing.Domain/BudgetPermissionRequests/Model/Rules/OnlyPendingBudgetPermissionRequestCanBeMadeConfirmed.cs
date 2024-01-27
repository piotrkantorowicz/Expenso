using System.Text;

using Expenso.BudgetSharing.Domain.BudgetPermissionRequests.Model.ValueObjects;
using Expenso.Shared.Domain.Types.Rules;

namespace Expenso.BudgetSharing.Domain.BudgetPermissionRequests.Model.Rules;

internal sealed class OnlyPendingBudgetPermissionRequestCanBeMadeConfirmed(
    BudgetPermissionRequestId budgetPermissionRequestId,
    BudgetPermissionRequestStatus status) : IBusinessRule
{
    private readonly BudgetPermissionRequestId _budgetPermissionRequestId =
        budgetPermissionRequestId ?? throw new ArgumentNullException(nameof(budgetPermissionRequestId));

    private readonly BudgetPermissionRequestStatus _status = status ?? throw new ArgumentNullException(nameof(status));

    public string Message => new StringBuilder()
        .Append("Only pending budget permission request - ")
        .Append(_budgetPermissionRequestId)
        .Append(" can be made confirmed")
        .ToString();

    public bool IsBroken()
    {
        return !_status.IsPending();
    }
}