using Expenso.Communication.Proxy.DTO.Settings.Email;
using Expenso.Shared.System.Configuration.Validators;
using Expenso.Shared.System.Types.TypesExtensions.Validations;

using Humanizer;

namespace Expenso.Api.Configuration.Settings.Services.Validators.Notifications;

internal sealed class SmtpSettingsValidator : ISettingsValidator<SmtpSettings>
{
    public IDictionary<string, string> Validate(SmtpSettings? settings)
    {
        Dictionary<string, string> errors = new();

        if (settings is null)
        {
            errors.Add(key: nameof(settings).Pascalize(), value: "SMTP settings are required.");

            return errors;
        }

        if (string.IsNullOrEmpty(value: settings.Host))
        {
            errors.Add(key: nameof(settings.Host), value: "SMTP host must be provided and cannot be empty.");
        }
        else if (!settings.Host.IsValidHost())
        {
            errors.Add(key: nameof(settings.Host), value: "SMTP host must be a valid DNS name, IPv4, or IPv6 address.");
        }

        if (settings.Port <= 0 || !settings.Port.IsValidPort())
        {
            errors.Add(key: nameof(settings.Port), value: "SMTP port must be a valid integer between 1 and 65535.");
        }

        if (string.IsNullOrEmpty(value: settings.Username))
        {
            errors.Add(key: nameof(settings.Username), value: "SMTP username must be provided and cannot be empty.");
        }
        else if (!settings.Username.IsValidUsername())
        {
            errors.Add(key: nameof(settings.Username),
                value: "SMTP username must be between 3 and 30 characters long and start with a letter.");
        }

        if (string.IsNullOrEmpty(value: settings.Password))
        {
            errors.Add(key: nameof(settings.Password), value: "SMTP password must be provided and cannot be empty.");
        }
        else if (!settings.Password.IsValidPassword(minLength: 8, maxLength: 20))
        {
            errors.Add(key: nameof(settings.Password),
                value:
                "SMTP password must be between 8 and 20 characters long, with at least one uppercase letter, one lowercase letter, one digit, and one special character.");
        }

        return errors;
    }
}