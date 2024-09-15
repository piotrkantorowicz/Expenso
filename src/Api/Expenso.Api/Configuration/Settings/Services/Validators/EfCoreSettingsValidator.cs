using Expenso.Shared.Database.EfCore.Settings;
using Expenso.Shared.System.Configuration.Validators;
using Expenso.Shared.System.Types.TypesExtensions.Validations;

namespace Expenso.Api.Configuration.Settings.Services.Validators;

internal sealed class EfCoreSettingsValidator : ISettingsValidator<EfCoreSettings>
{
    public IDictionary<string, string> Validate(EfCoreSettings? settings)
    {
        Dictionary<string, string> errors = new();

        if (settings is null)
        {
            errors.Add(key: nameof(settings), value: "Settings are required");

            return errors;
        }

        if (settings.ConnectionParameters is null)
        {
            errors.Add(key: nameof(settings.ConnectionParameters),
                value: "ConnectionParameters must be provided and cannot be null");
        }
        else
        {
            ValidateConnectionParameters(connectionParameters: settings.ConnectionParameters, errors: errors);
        }

        if (settings.InMemory is null)
        {
            errors.Add(key: nameof(settings.InMemory), value: "InMemory flag must be provided");
        }

        if (settings.UseMigration is null)
        {
            errors.Add(key: nameof(settings.UseMigration), value: "UseMigration flag must be provided");
        }

        if (settings.UseSeeding is null)
        {
            errors.Add(key: nameof(settings.UseSeeding), value: "UseSeeding flag must be provided");
        }

        return errors;
    }

    private static void ValidateConnectionParameters(ConnectionParameters? connectionParameters,
        Dictionary<string, string> errors)
    {
        if (string.IsNullOrEmpty(value: connectionParameters?.Host))
        {
            errors.Add(key: nameof(connectionParameters.Host), value: "Host must be provided and cannot be empty");
        }
        else if (!connectionParameters.Host.IsValidHost())
        {
            errors.Add(key: nameof(connectionParameters.Host), value: "Host must be a valid DNS name or IP address");
        }

        if (string.IsNullOrEmpty(value: connectionParameters?.Port))
        {
            errors.Add(key: nameof(connectionParameters.Port), value: "Port must be provided and cannot be empty");
        }
        else if (!int.TryParse(s: connectionParameters.Port, result: out int portNumber) ||
                 portNumber is < 1 or > 65535)
        {
            errors.Add(key: nameof(connectionParameters.Port),
                value: "Port must be a valid integer between 1 and 65535");
        }

        if (string.IsNullOrEmpty(value: connectionParameters?.DefaultDatabase))
        {
            errors.Add(key: nameof(connectionParameters.DefaultDatabase),
                value: "DefaultDatabase must be provided and cannot be empty");
        }
        else if (!connectionParameters.DefaultDatabase.IsAlphaNumericAndSpecialCharactersString(minLength: 1,
                     maxLength: 100))
        {
            errors.Add(key: nameof(connectionParameters.DefaultDatabase),
                value: "DefaultDatabase must be an alphanumeric string between 1 and 100 characters");
        }

        if (string.IsNullOrEmpty(value: connectionParameters?.Database))
        {
            errors.Add(key: nameof(connectionParameters.Database),
                value: "Database must be provided and cannot be empty");
        }
        else if (!connectionParameters.Database.IsAlphaNumericAndSpecialCharactersString(minLength: 1, maxLength: 100))
        {
            errors.Add(key: nameof(connectionParameters.Database),
                value: "Database must be an alphanumeric string between 1 and 100 characters");
        }

        if (string.IsNullOrEmpty(value: connectionParameters?.User))
        {
            errors.Add(key: nameof(connectionParameters.User), value: "User must be provided and cannot be empty");
        }
        else if (!connectionParameters.User.IsValidUsername())
        {
            errors.Add(key: nameof(connectionParameters.User),
                value:
                "User must be a valid alphanumeric string starting with a letter and between 3 and 30 characters");
        }

        if (string.IsNullOrEmpty(value: connectionParameters?.Password))
        {
            errors.Add(key: nameof(connectionParameters.Password),
                value: "Password must be provided and cannot be empty");
        }
        else if (!connectionParameters.Password.IsValidPassword(minLength: 8, maxLength: 30))
        {
            errors.Add(key: nameof(connectionParameters.Password),
                value:
                "Password must be between 8 and 30 characters, contain an upper and lower case letter, a digit, and a special character, with no spaces");
        }
    }
}