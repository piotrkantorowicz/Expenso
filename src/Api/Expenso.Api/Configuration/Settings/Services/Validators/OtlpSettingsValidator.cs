using Expenso.Shared.System.Configuration.Validators;
using Expenso.Shared.System.Metrics;
using Expenso.Shared.System.Types.TypesExtensions.Validations;

using Humanizer;

namespace Expenso.Api.Configuration.Settings.Services.Validators;

internal sealed class OtlpSettingsValidator : ISettingsValidator<OtlpSettings>
{
    public IDictionary<string, string> Validate(OtlpSettings? settings)
    {
        Dictionary<string, string> errors = new();

        if (settings is null)
        {
            errors.Add(key: nameof(settings).Pascalize(), value: "Otlp settings are required");

            return errors;
        }

        if (string.IsNullOrEmpty(value: settings.ServiceName))
        {
            errors.Add(key: nameof(settings.ServiceName), value: "Service name must be provided and cannot be empty");
        }
        else
        {
            if (!settings.ServiceName.IsAlphaNumericAndSpecialCharactersString(specialCharacters: "_.-"))
            {
                errors.Add(key: nameof(settings.ServiceName),
                    value: "Service name can only contain alphanumeric characters and special characters");
            }
        }

        if (string.IsNullOrEmpty(value: settings.Endpoint))
        {
            errors.Add(key: nameof(settings.Endpoint), value: "Endpoint must be provided and cannot be empty");
        }
        else
        {
            if (!settings.Endpoint.IsValidUrl())
            {
                errors.Add(key: nameof(settings.Endpoint), value: "Endpoint must be a valid URL");
            }
        }

        return errors;
    }
}