using Expenso.Communication.Shared.DTO.Settings.Email;
using Expenso.Shared.System.Configuration.Validators;
using Expenso.Shared.System.Types.TypesExtensions;
using Expenso.Shared.System.Types.TypesExtensions.Validations;

using Humanizer;

namespace Expenso.Api.Configuration.Settings.Services.Validators.Notifications;

internal sealed class EmailNotificationSettingsValidator : ISettingsValidator<EmailNotificationSettings>
{
    public IDictionary<string, string> Validate(EmailNotificationSettings? settings)
    {
        Dictionary<string, string> errors = new();

        if (settings is null)
        {
            errors.Add(key: nameof(settings).Pascalize(), value: "Email notification settings are required.");

            return errors;
        }

        if (settings.Enabled is null)
        {
            errors.Add(key: nameof(settings.Enabled), value: "Email enabled flag must be provided.");

            return errors;
        }

        if (settings.Enabled is false)
        {
            return errors;
        }

        if (string.IsNullOrWhiteSpace(value: settings.From))
        {
            errors.Add(key: nameof(settings.From), value: "Email 'From' address must be provided and cannot be empty.");
        }
        else if (!settings.From.IsValidEmail())
        {
            errors.Add(key: nameof(settings.From), value: "Email 'From' address must be a valid email address.");
        }

        if (string.IsNullOrWhiteSpace(value: settings.ReplyTo))
        {
            errors.Add(key: nameof(settings.ReplyTo),
                value: "Email 'ReplyTo' address must be provided and cannot be empty.");
        }
        else if (!settings.ReplyTo.IsValidEmail())
        {
            errors.Add(key: nameof(settings.ReplyTo), value: "Email 'ReplyTo' address must be a valid email address.");
        }

        if (settings.Smtp is null)
        {
            errors.Add(key: nameof(settings.Smtp), value: "Smtp must be provided and cannot be null.");
        }
        else
        {
            errors.Merge(items: new SmtpSettingsValidator().Validate(settings: settings.Smtp));
        }

        return errors;
    }
}