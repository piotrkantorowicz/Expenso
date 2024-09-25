using Expenso.Shared.System.Configuration.Validators;
using Expenso.Shared.System.Types.TypesExtensions.Validations;

using Humanizer;

namespace Expenso.Api.Configuration.Settings.Services.Validators;

internal sealed class CorsSettingsValidator : ISettingsValidator<CorsSettings>
{
    public IDictionary<string, string> Validate(CorsSettings? settings)
    {
        Dictionary<string, string> errors = new();

        if (settings is null)
        {
            errors.Add(key: nameof(settings).Pascalize(), value: "Cors settings are required.");

            return errors;
        }

        if (settings.Enabled is null)
        {
            errors.Add(key: nameof(settings.Enabled), value: "Cors enabled flag must be provided.");

            return errors;
        }

        if (settings is { Enabled: true, AllowedOrigins: null or [] })
        {
            errors.Add(key: nameof(settings.AllowedOrigins), value: "AllowedOrigins cannot be null or empty.");
        }
        else if (settings is { Enabled: true, AllowedOrigins.Length: > 0 })
        {
            foreach (string allowedOrigin in settings.AllowedOrigins)
            {
                if (string.IsNullOrEmpty(value: allowedOrigin) ||
                    (allowedOrigin is not "*" && !allowedOrigin.IsValidUrl()))
                {
                    errors.Add(key: nameof(settings.AllowedOrigins),
                        value: "Origin cannot be empty and must be a valid URL.");
                }
            }
        }

        return errors;
    }
}