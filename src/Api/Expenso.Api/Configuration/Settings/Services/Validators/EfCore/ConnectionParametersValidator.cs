using Expenso.Shared.Database.EfCore.Settings;
using Expenso.Shared.System.Configuration.Validators;
using Expenso.Shared.System.Types.TypesExtensions.Validations;

namespace Expenso.Api.Configuration.Settings.Services.Validators.EfCore;

internal sealed class ConnectionParametersValidator : ISettingsValidator<ConnectionParameters>
{
    public IDictionary<string, string> Validate(ConnectionParameters settings)
    {
        Dictionary<string, string> errors = new();

        if (string.IsNullOrEmpty(value: settings?.Host))
        {
            errors.Add(key: nameof(settings.Host), value: "Host must be provided and cannot be empty");
        }
        else if (!settings.Host.IsValidHost())
        {
            errors.Add(key: nameof(settings.Host), value: "Host must be a valid DNS name or IP address");
        }

        if (string.IsNullOrEmpty(value: settings?.Port))
        {
            errors.Add(key: nameof(settings.Port), value: "Port must be provided and cannot be empty");
        }
        else if (!int.TryParse(s: settings.Port, result: out int portNumber) || portNumber is < 1 or > 65535)
        {
            errors.Add(key: nameof(settings.Port), value: "Port must be a valid integer between 1 and 65535");
        }

        if (string.IsNullOrEmpty(value: settings?.DefaultDatabase))
        {
            errors.Add(key: nameof(settings.DefaultDatabase),
                value: "DefaultDatabase must be provided and cannot be empty");
        }
        else if (!settings.DefaultDatabase.IsAlphaNumericAndSpecialCharactersString(minLength: 1, maxLength: 100))
        {
            errors.Add(key: nameof(settings.DefaultDatabase),
                value: "DefaultDatabase must be an alphanumeric string between 1 and 100 characters");
        }

        if (string.IsNullOrEmpty(value: settings?.Database))
        {
            errors.Add(key: nameof(settings.Database), value: "Database must be provided and cannot be empty");
        }
        else if (!settings.Database.IsAlphaNumericAndSpecialCharactersString(minLength: 1, maxLength: 100))
        {
            errors.Add(key: nameof(settings.Database),
                value: "Database must be an alphanumeric string between 1 and 100 characters");
        }

        if (string.IsNullOrEmpty(value: settings?.User))
        {
            errors.Add(key: nameof(settings.User), value: "User must be provided and cannot be empty");
        }
        else if (!settings.User.IsValidUsername())
        {
            errors.Add(key: nameof(settings.User),
                value:
                "User must be a valid alphanumeric string starting with a letter and between 3 and 30 characters");
        }

        if (string.IsNullOrEmpty(value: settings?.Password))
        {
            errors.Add(key: nameof(settings.Password), value: "Password must be provided and cannot be empty");
        }
        else if (!settings.Password.IsValidPassword(minLength: 8, maxLength: 30))
        {
            errors.Add(key: nameof(settings.Password),
                value:
                "Password must be between 8 and 30 characters, contain an upper and lower case letter, a digit, and a special character, with no spaces");
        }

        return errors;
    }
}