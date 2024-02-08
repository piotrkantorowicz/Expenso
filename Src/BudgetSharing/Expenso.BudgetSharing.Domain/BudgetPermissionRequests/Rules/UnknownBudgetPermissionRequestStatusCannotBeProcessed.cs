using System.Text;

using Expenso.BudgetSharing.Domain.BudgetPermissionRequests.ValueObjects;
using Expenso.Shared.Domain.Types.Rules;

namespace Expenso.BudgetSharing.Domain.BudgetPermissionRequests.Rules;

internal sealed class UnknownBudgetPermissionRequestStatusCannotBeProcessed(BudgetPermissionRequestStatus status)
    : IBusinessRule
{
    private readonly BudgetPermissionRequestStatus _status = status ?? throw new ArgumentNullException(nameof(status));

    public string Message => new StringBuilder()
        .Append("Unknown budget permission request status - ")
        .Append(_status)
        .Append(" cannot be processed")
        .ToString();

    public bool IsBroken()
    {
        return _status.IsUnknown();
    }
}