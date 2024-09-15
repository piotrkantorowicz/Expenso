using Expenso.Communication.Proxy.DTO.Settings;
using Expenso.Communication.Proxy.DTO.Settings.Email;
using Expenso.Communication.Proxy.DTO.Settings.InApp;
using Expenso.Communication.Proxy.DTO.Settings.Push;
using Expenso.Shared.System.Configuration.Validators;
using Expenso.Shared.System.Types.TypesExtensions;
using Expenso.Shared.System.Types.TypesExtensions.Validations;

namespace Expenso.Api.Configuration.Settings.Services.Validators;

internal sealed class NotificationSettingsValidator : ISettingsValidator<NotificationSettings>
{
    public IDictionary<string, string> Validate(NotificationSettings? settings)
    {
        Dictionary<string, string> errors = new();

        if (settings is null)
        {
            errors.Add(key: nameof(settings), value: "Settings are required");

            return errors;
        }

        if (settings.Enabled is null)
        {
            errors.Add(key: nameof(settings.Enabled), value: "Settings are required");

            return errors;
        }

        if (settings.Enabled is false)
        {
            return errors;
        }

        if (settings.Email is null)
        {
            errors.Add(key: nameof(settings.Email), value: "Email notification settings must be provided");
        }
        else
        {
            errors.Merge(items: ValidateEmailNotificationSettings(emailSettings: settings.Email));
        }

        if (settings.InApp is null)
        {
            errors.Add(key: nameof(settings.InApp), value: "In-app notification settings must be provided");
        }
        else
        {
            errors.Merge(items: ValidateInAppNotificationSettings(inAppSettings: settings.InApp));
        }

        if (settings.Push is null)
        {
            errors.Add(key: nameof(settings.Push), value: "Push notification settings must be provided");
        }
        else
        {
            errors.Merge(items: ValidatePushNotificationSettings(pushSettings: settings.Push));
        }

        return errors;
    }

    private static Dictionary<string, string> ValidateEmailNotificationSettings(
        EmailNotificationSettings? emailSettings)
    {
        Dictionary<string, string> errors = new();

        if (emailSettings is null)
        {
            errors.Add(key: nameof(emailSettings), value: "Email settings are required");

            return errors;
        }

        if (emailSettings.Enabled is null)
        {
            errors.Add(key: nameof(emailSettings.Enabled), value: "Email enabled flag must be provided");

            return errors;
        }

        if (emailSettings.Enabled is false)
        {
            return errors;
        }

        if (string.IsNullOrEmpty(value: emailSettings.From))
        {
            errors.Add(key: nameof(emailSettings.From),
                value: "Email 'From' address must be provided and cannot be empty");
        }
        else
        {
            if (!emailSettings.From.IsValidEmail())
            {
                errors.Add(key: nameof(emailSettings.From),
                    value: "Email 'From' address must be a valid email address");
            }
        }

        if (string.IsNullOrEmpty(value: emailSettings.ReplyTo))
        {
            errors.Add(key: nameof(emailSettings.ReplyTo),
                value: "Email 'ReplyTo' address must be provided and cannot be empty");
        }
        else
        {
            if (!emailSettings.ReplyTo.IsValidEmail())
            {
                errors.Add(key: nameof(emailSettings.ReplyTo),
                    value: "Email 'ReplyTo' address must be a valid email address");
            }
        }

        errors.Merge(items: ValidateSmtpSettings(smtpSettings: emailSettings.Smtp));

        return errors;
    }

    private static Dictionary<string, string> ValidateSmtpSettings(SmtpSettings? smtpSettings)
    {
        Dictionary<string, string> errors = new();

        if (smtpSettings is null)
        {
            errors.Add(key: nameof(smtpSettings), value: "Smtp settings are required");

            return errors;
        }

        if (string.IsNullOrEmpty(value: smtpSettings.Host))
        {
            errors.Add(key: nameof(smtpSettings.Host), value: "SMTP host must be provided and cannot be empty");
        }
        else
        {
            if (!smtpSettings.Host.IsValidHost())
            {
                errors.Add(key: nameof(smtpSettings.Host),
                    value: "SMTP host must be a valid DNS name, IPv4, or IPv6 address");
            }
        }

        if (smtpSettings.Port <= 0)
        {
            errors.Add(key: nameof(smtpSettings.Port), value: "SMTP port must be a positive integer");
        }
        else
        {
            if (!smtpSettings.Port.IsValidPort())
            {
                errors.Add(key: nameof(smtpSettings.Port),
                    value: "SMTP port must be a valid integer between 1 and 65535");
            }
        }

        if (string.IsNullOrEmpty(value: smtpSettings.Username))
        {
            errors.Add(key: nameof(smtpSettings.Username), value: "SMTP username must be provided and cannot be empty");
        }
        else
        {
            if (!smtpSettings.Username.IsValidUsername())
            {
                errors.Add(key: nameof(smtpSettings.Username),
                    value: "SMTP username must be between 3 and 30 characters long and start with a letter");
            }
        }

        if (string.IsNullOrEmpty(value: smtpSettings.Password))
        {
            errors.Add(key: nameof(smtpSettings.Password), value: "SMTP password must be provided and cannot be empty");
        }
        else
        {
            if (!smtpSettings.Password.IsValidPassword(minLength: 8, maxLength: 20))
            {
                errors.Add(key: nameof(smtpSettings.Password),
                    value:
                    "SMTP password must be between 8 and 20 characters long, and include at least one uppercase letter, one lowercase letter, one digit, and one special character");
            }
        }

        return errors;
    }

    private static Dictionary<string, string> ValidateInAppNotificationSettings(
        InAppNotificationSettings? inAppSettings)
    {
        Dictionary<string, string> errors = new();

        if (inAppSettings is null)
        {
            errors.Add(key: nameof(inAppSettings), value: "In app settings are required");

            return errors;
        }

        if (inAppSettings.Enabled is null)
        {
            errors.Add(key: nameof(inAppSettings.Enabled), value: "InApp enabled flag must be provided");
        }

        if (inAppSettings.Enabled is false)
        {
            return errors;
        }

        return errors;
    }

    private static Dictionary<string, string> ValidatePushNotificationSettings(PushNotificationSettings? pushSettings)
    {
        Dictionary<string, string> errors = new();

        if (pushSettings is null)
        {
            errors.Add(key: nameof(pushSettings), value: "Push settings are required");

            return errors;
        }

        if (pushSettings.Enabled is null)
        {
            errors.Add(key: nameof(pushSettings.Enabled), value: "Push enabled flag must be provided");
        }

        if (pushSettings.Enabled is false)
        {
            return errors;
        }

        return errors;
    }
}