using Expenso.Communication.Shared.DTO.Settings;
using Expenso.Shared.System.Configuration.Validators;
using Expenso.Shared.System.Types.TypesExtensions;

using Humanizer;

namespace Expenso.Api.Configuration.Settings.Services.Validators.Notifications;

internal sealed class NotificationSettingsValidator : ISettingsValidator<NotificationSettings>
{
    public IDictionary<string, string> Validate(NotificationSettings? settings)
    {
        Dictionary<string, string> errors = new();

        if (settings is null)
        {
            errors.Add(key: nameof(settings).Pascalize(), value: "Notification settings are required.");

            return errors;
        }

        if (settings.Enabled is null)
        {
            errors.Add(key: nameof(settings.Enabled), value: "Enabled flag must be provided.");

            return errors;
        }

        if (settings.Enabled is false)
        {
            return errors;
        }

        if (settings.Email is null)
        {
            errors.Add(key: nameof(settings.Email), value: "Email notification settings must be provided.");
        }
        else
        {
            errors.Merge(items: new EmailNotificationSettingsValidator().Validate(settings: settings.Email));
        }

        if (settings.InApp is null)
        {
            errors.Add(key: nameof(settings.InApp), value: "In-app notification settings must be provided.");
        }
        else
        {
            errors.Merge(items: new InAppNotificationSettingsValidator().Validate(settings: settings.InApp));
        }

        if (settings.Push is null)
        {
            errors.Add(key: nameof(settings.Push), value: "Push notification settings must be provided.");
        }
        else
        {
            errors.Merge(items: new PushNotificationSettingsValidator().Validate(settings: settings.Push));
        }

        return errors;
    }
}