using Expenso.BudgetSharing.Application.Shared.Settings;
using Expenso.Shared.System.Configuration.Validators;

using Humanizer;

namespace Expenso.Api.Configuration.Settings.Services.Validators;

internal sealed class BudgetSharingSettingsValidator : ISettingsValidator<BudgetSharingSettings>
{
    public IDictionary<string, string> Validate(BudgetSharingSettings? settings)
    {
        Dictionary<string, string> errors = new();

        if (settings is null)
        {
            errors.Add(key: nameof(settings).Pascalize(), value: "Budget sharing settings are required.");

            return errors;
        }

        if (settings.ExpirationDays is <= 0 or > 7)
        {
            errors.Add(key: nameof(settings.ExpirationDays),
                value: "Expiration days must greater than 0 and less than 7.");
        }

        return errors;
    }
}