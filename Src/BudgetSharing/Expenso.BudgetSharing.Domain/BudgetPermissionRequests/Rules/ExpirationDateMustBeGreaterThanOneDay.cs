using Expenso.Shared.Domain.Types.Rules;
using Expenso.Shared.Domain.Types.ValueObjects;
using Expenso.Shared.System.Types.Clock;

namespace Expenso.BudgetSharing.Domain.BudgetPermissionRequests.Rules;

internal sealed class ExpirationDateMustBeGreaterThanOneDay(DateAndTime expirationDate, IClock clock) : IBusinessRule
{
    public string Message => $"Expiration date {expirationDate.Value} must be greater than one day.";

    public bool IsBroken()
    {
        return expirationDate < clock.UtcNow.AddDays(days: 1);
    }
}