using Expenso.BudgetSharing.Domain.BudgetPermissionRequests.ValueObjects;
using Expenso.Shared.Domain.Types.Rules;

namespace Expenso.BudgetSharing.Domain.BudgetPermissionRequests.Rules;

internal sealed class UnknownBudgetPermissionRequestStatusCannotBeProcessed : IBusinessRule
{
    private readonly BudgetPermissionRequestStatus _status;

    public UnknownBudgetPermissionRequestStatusCannotBeProcessed(BudgetPermissionRequestStatus status)
    {
        _status = status ?? throw new ArgumentNullException(paramName: nameof(status));
    }

    public string Message => $"Unknown budget permission request status {_status} cannot be processed";

    public bool IsBroken()
    {
        return _status.IsUnknown();
    }
}