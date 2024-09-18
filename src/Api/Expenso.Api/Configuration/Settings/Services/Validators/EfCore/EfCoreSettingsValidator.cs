using Expenso.Shared.Database.EfCore.Settings;
using Expenso.Shared.System.Configuration.Validators;
using Expenso.Shared.System.Types.TypesExtensions;

using Humanizer;

namespace Expenso.Api.Configuration.Settings.Services.Validators.EfCore;

internal sealed class EfCoreSettingsValidator : ISettingsValidator<EfCoreSettings>
{
    public IDictionary<string, string> Validate(EfCoreSettings? settings)
    {
        Dictionary<string, string> errors = new();

        if (settings is null)
        {
            errors.Add(key: nameof(settings).Pascalize().Pascalize(), value: "EfCore settings are required");

            return errors;
        }

        if (settings.ConnectionParameters is null)
        {
            errors.Add(key: nameof(settings.ConnectionParameters),
                value: "ConnectionParameters must be provided and cannot be null");
        }
        else
        {
            errors.Merge(items: new ConnectionParametersValidator().Validate(settings: settings.ConnectionParameters));
        }

        if (settings.InMemory is null)
        {
            errors.Add(key: nameof(settings.InMemory), value: "InMemory flag must be provided");
        }

        if (settings.UseMigration is null)
        {
            errors.Add(key: nameof(settings.UseMigration), value: "UseMigration flag must be provided");
        }

        if (settings.UseSeeding is null)
        {
            errors.Add(key: nameof(settings.UseSeeding), value: "UseSeeding flag must be provided");
        }

        return errors;
    }
}