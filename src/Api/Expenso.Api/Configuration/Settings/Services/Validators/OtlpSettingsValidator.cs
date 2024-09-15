using Expenso.Shared.System.Configuration.Validators;
using Expenso.Shared.System.Metrics;

namespace Expenso.Api.Configuration.Settings.Services.Validators;

internal sealed class OtlpSettingsValidator : ISettingsValidator<OtlpSettings>
{
    public IDictionary<string, string> Validate(OtlpSettings? settings)
    {
        Dictionary<string, string> errors = new();

        if (settings is null)
        {
            errors.Add(key: nameof(settings), value: "Settings are required");

            return errors;
        }

        if (string.IsNullOrEmpty(value: settings.ServiceName))
        {
            errors.Add(key: nameof(settings.ServiceName), value: "Service name must be provided and cannot be empty");
        }

        if (string.IsNullOrEmpty(value: settings.Endpoint))
        {
            errors.Add(key: nameof(settings.Endpoint), value: "Endpoint must be provided and cannot be empty");
        }

        return errors;
    }
}