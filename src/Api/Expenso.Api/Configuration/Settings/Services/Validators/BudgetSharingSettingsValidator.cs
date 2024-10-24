using Expenso.BudgetSharing.Application.Shared.Settings;
using Expenso.Shared.System.Configuration.Validators;

using Humanizer;

namespace Expenso.Api.Configuration.Settings.Services.Validators;

internal sealed class BudgetSharingSettingsValidator : ISettingsValidator<BudgetSharingSettings>
{
    private const int MinExpirationDays = 1;
    private const int MaxExpirationDays = 7;

    public IDictionary<string, string> Validate(BudgetSharingSettings? settings)
    {
        Dictionary<string, string> errors = new();

        if (settings is null)
        {
            errors.Add(key: nameof(settings).Pascalize(), value: "Budget sharing settings are required.");

            return errors;
        }

        if (settings.ExpirationDays is <= MinExpirationDays or > MaxExpirationDays)
        {
            errors.Add(key: nameof(settings.ExpirationDays), value: "Expiration days must be between 1 and 7 days.");
        }

        return errors;
    }
}