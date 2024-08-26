using Expenso.Shared.Domain.Types.Rules;
using Expenso.Shared.Domain.Types.ValueObjects;
using Expenso.Shared.System.Types.Clock;

namespace Expenso.BudgetSharing.Domain.BudgetPermissionRequests.Rules;

internal sealed class ExpirationDateMustBeGreaterThanOneDay : IBusinessRule
{
    private readonly IClock _clock;
    private readonly DateAndTime _expirationDate;

    public ExpirationDateMustBeGreaterThanOneDay(DateAndTime expirationDate, IClock clock)
    {
        _expirationDate = expirationDate;
        _clock = clock;
    }

    public string Message => $"Expiration date {_expirationDate.Value} must be greater than one day";

    public bool IsBroken()
    {
        return _expirationDate.LessThanOrEqual(dateTimeOffset: _clock.UtcNow.AddDays(days: 1));
    }
}