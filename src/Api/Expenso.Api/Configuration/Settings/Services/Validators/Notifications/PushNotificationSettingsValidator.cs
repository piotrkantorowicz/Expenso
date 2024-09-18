using Expenso.Communication.Proxy.DTO.Settings.Push;
using Expenso.Shared.System.Configuration.Validators;

using Humanizer;

namespace Expenso.Api.Configuration.Settings.Services.Validators.Notifications;

internal sealed class PushNotificationSettingsValidator : ISettingsValidator<PushNotificationSettings>
{
    public IDictionary<string, string> Validate(PushNotificationSettings? settings)
    {
        Dictionary<string, string> errors = new();

        if (settings is null)
        {
            errors.Add(key: nameof(settings).Pascalize(), value: "Push notification settings are required");

            return errors;
        }

        if (settings.Enabled is null)
        {
            errors.Add(key: nameof(settings.Enabled), value: "Push enabled flag must be provided");
        }

        return errors;
    }
}