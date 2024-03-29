using Expenso.BudgetSharing.Domain.BudgetPermissionRequests.ValueObjects;
using Expenso.Shared.Domain.Types.Rules;

namespace Expenso.BudgetSharing.Domain.BudgetPermissionRequests.Rules;

internal sealed class UnknownBudgetPermissionRequestStatusCannotBeProcessed(BudgetPermissionRequestStatus status)
    : IBusinessRule
{
    private readonly BudgetPermissionRequestStatus _status = status ?? throw new ArgumentNullException(nameof(status));

    public string Message => $"Unknown budget permission request status {_status} cannot be processed.";

    public bool IsBroken()
    {
        return _status.IsUnknown();
    }
}