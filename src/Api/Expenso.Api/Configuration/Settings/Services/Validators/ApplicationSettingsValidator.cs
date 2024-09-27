using Expenso.Shared.System.Configuration.Settings.App;
using Expenso.Shared.System.Configuration.Validators;

using Humanizer;

namespace Expenso.Api.Configuration.Settings.Services.Validators;

internal sealed class ApplicationSettingsValidator : ISettingsValidator<ApplicationSettings>
{
    public IDictionary<string, string> Validate(ApplicationSettings? settings)
    {
        Dictionary<string, string> errors = new();

        if (settings is null)
        {
            errors.Add(key: nameof(settings).Pascalize(), value: "Application settings are required.");

            return errors;
        }

        if (settings.InstanceId is null || settings.InstanceId == Guid.Empty)
        {
            errors.Add(key: nameof(settings.InstanceId), value: "Instance Id must be provided and cannot be empty.");
        }

        if (string.IsNullOrEmpty(value: settings.Name))
        {
            errors.Add(key: nameof(settings.Name), value: "Name must be provided and cannot be empty.");
        }

        if (string.IsNullOrEmpty(value: settings.Version))
        {
            errors.Add(key: nameof(settings.Version), value: "Version must be provided and cannot be empty.");
        }
        else
        {
            string? assemblyVersion = typeof(Program).Assembly.GetName().Version?.ToString();

            if (Version.TryParse(input: settings.Version, result: out Version? settingsVer) &&
                Version.TryParse(input: assemblyVersion, result: out Version? assemblyVer) &&
                (settingsVer.Major != assemblyVer.Major || settingsVer.Minor != assemblyVer.Minor ||
                 settingsVer.Build != assemblyVer.Build))
            {
                errors.Add(key: nameof(settings.Version),
                    value:
                    $"Version mismatch. Expected: [{assemblyVer.Major}.{assemblyVer.Minor}.{assemblyVer.Build}], but got: [{settingsVer.Major}.{settingsVer.Minor}.{settingsVer.Build}].");
            }
        }

        return errors;
    }
}