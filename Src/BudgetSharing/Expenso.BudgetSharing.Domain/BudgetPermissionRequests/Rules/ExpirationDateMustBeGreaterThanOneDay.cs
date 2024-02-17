using System.Text;

using Expenso.Shared.Domain.Types.Rules;
using Expenso.Shared.Domain.Types.ValueObjects;
using Expenso.Shared.System.Types.Clock;

namespace Expenso.BudgetSharing.Domain.BudgetPermissionRequests.Rules;

internal sealed class ExpirationDateMustBeGreaterThanOneDay(DateAndTime expirationDate, IClock clock) : IBusinessRule
{
    public string Message => new StringBuilder()
        .Append("Expiration date - ")
        .Append(expirationDate)
        .Append(" must be greater than one day.")
        .ToString();

    public bool IsBroken()
    {
        return expirationDate < clock.UtcNow.AddDays(1);
    }
}