using Expenso.Communication.Shared.DTO.Settings.InApp;
using Expenso.Shared.System.Configuration.Validators;

using Humanizer;

namespace Expenso.Api.Configuration.Settings.Services.Validators.Notifications;

internal sealed class InAppNotificationSettingsValidator : ISettingsValidator<InAppNotificationSettings>
{
    public IDictionary<string, string> Validate(InAppNotificationSettings? settings)
    {
        Dictionary<string, string> errors = new();

        if (settings is null)
        {
            errors.Add(key: nameof(settings).Pascalize(), value: "In-app notification settings are required.");

            return errors;
        }

        if (settings.Enabled is null)
        {
            errors.Add(key: nameof(settings.Enabled), value: "In-app enabled flag must be provided.");
        }

        return errors;
    }
}