using Expenso.Shared.System.Configuration.Settings.Auth;
using Expenso.Shared.System.Configuration.Validators;

namespace Expenso.Api.Configuration.Settings.Services.Validators;

internal sealed class AuthSettingsValidator : ISettingsValidator<AuthSettings>
{
    public IDictionary<string, string> Validate(AuthSettings? settings)
    {
        Dictionary<string, string> errors = new();

        if (settings is null)
        {
            errors.Add(key: nameof(settings), value: "Settings are required");

            return errors;
        }

        if (!Enum.IsDefined(enumType: typeof(AuthServer), value: settings.AuthServer))
        {
            errors.Add(key: nameof(settings.AuthServer), value: "AuthServer must be a valid value");
        }

        return errors;
    }
}