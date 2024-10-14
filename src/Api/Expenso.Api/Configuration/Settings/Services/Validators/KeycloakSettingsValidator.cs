using Expenso.IAM.Core.Acl.Keycloak;
using Expenso.Shared.System.Configuration.Validators;
using Expenso.Shared.System.Types.TypesExtensions.Validations;

using Humanizer;

namespace Expenso.Api.Configuration.Settings.Services.Validators;

internal sealed class KeycloakSettingsValidator : ISettingsValidator<KeycloakSettings>
{
    private readonly string[] _sslRequiredValues = ["ALL", "EXTERNAL", "NONE"];

    public IDictionary<string, string> Validate(KeycloakSettings? settings)
    {
        Dictionary<string, string> errors = new();

        if (settings is null)
        {
            errors.Add(key: nameof(settings).Pascalize(), value: "Keycloak settings are required.");

            return errors;
        }

        if (string.IsNullOrWhiteSpace(value: settings.AuthServerUrl))
        {
            errors.Add(key: nameof(settings.AuthServerUrl),
                value: "Authorization server URL must be provided and cannot be empty.");
        }
        else
        {
            if (!settings.AuthServerUrl.IsValidUrl())
            {
                errors.Add(key: nameof(settings.AuthServerUrl),
                    value: "Authorization server URL must be a valid HTTP or HTTPS URL.");
            }
        }

        if (string.IsNullOrWhiteSpace(value: settings.Realm))
        {
            errors.Add(key: nameof(settings.Realm), value: "Realm must be provided and cannot be empty.");
        }
        else
        {
            if (!settings.Realm.IsAlphaNumericString(minLength: 5, maxLength: 50))
            {
                errors.Add(key: nameof(settings.Realm),
                    value: "Realm must be an alpha string with a length between 5 and 50 characters.");
            }
        }

        if (string.IsNullOrWhiteSpace(value: settings.Resource))
        {
            errors.Add(key: nameof(settings.Resource),
                value: "Resource (client ID) must be provided and cannot be empty.");
        }
        else
        {
            if (!settings.Resource.IsAlphaNumericAndSpecialCharactersString(minLength: 5, maxLength: 100,
                    specialCharacters: "_.-"))
            {
                errors.Add(key: nameof(settings.Resource),
                    value: "Resource (client ID) must be an alpha string with a length between 5 and 100 characters.");
            }
        }

        if (string.IsNullOrWhiteSpace(value: settings.SslRequired))
        {
            errors.Add(key: nameof(settings.SslRequired),
                value: "SSL requirement must be specified and cannot be empty.");
        }
        else
        {
            if (!_sslRequiredValues.Contains(value: settings.SslRequired,
                    comparer: StringComparer.InvariantCultureIgnoreCase))
            {
                errors.Add(key: nameof(settings.SslRequired),
                    value: "SSL requirement must be one of the predefined values.");
            }
        }

        if (settings.VerifyTokenAudience == null)
        {
            errors.Add(key: nameof(settings.VerifyTokenAudience), value: "VerifyTokenAudience must be specified.");
        }

        /* ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
           It's inherited object from Keycloak library, but because it is binding from .appsetting
           it for sure can be null in some cases */
        if (settings.Credentials == null)
        {
            errors.Add(key: nameof(settings.Credentials), value: "Client secret must be provided and cannot be empty.");

            return errors;
        }

        if (string.IsNullOrWhiteSpace(value: settings.Credentials.Secret))
        {
            errors.Add(key: nameof(settings.Credentials.Secret),
                value: "Client secret must be provided and cannot be empty.");
        }
        else
        {
            if (!Guid.TryParse(input: settings.Credentials.Secret, result: out Guid _))
            {
                errors.Add(key: nameof(settings.Credentials.Secret),
                    value: "Client secret must be a valid GUID format.");
            }
        }

        return errors;
    }
}