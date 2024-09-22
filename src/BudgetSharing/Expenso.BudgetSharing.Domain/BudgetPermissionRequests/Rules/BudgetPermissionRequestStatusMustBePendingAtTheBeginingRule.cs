using Expenso.BudgetSharing.Domain.BudgetPermissionRequests.ValueObjects;
using Expenso.Shared.Domain.Types.Rules;

namespace Expenso.BudgetSharing.Domain.BudgetPermissionRequests.Rules;

internal sealed class BudgetPermissionRequestStatusMustBePendingAtTheBeginingRule : IBusinessRule
{
    private readonly BudgetPermissionRequestStatus _status;

    public BudgetPermissionRequestStatusMustBePendingAtTheBeginingRule(BudgetPermissionRequestStatus status)
    {
        _status = status ?? throw new ArgumentNullException(paramName: nameof(status));
    }

    public string Message => $"Budget permission request status must be 'Pending' but was '{_status}'.";

    public bool IsBroken()
    {
        return !_status.IsPending();
    }
}