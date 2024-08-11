using Expenso.Shared.Commands.Validation;

namespace Expenso.UserPreferences.Core.Application.Preferences.Write.Commands.CreatePreference;

internal sealed class CreatePreferenceCommandValidator : ICommandValidator<CreatePreferenceCommand>
{
    public IDictionary<string, string> Validate(CreatePreferenceCommand? command)
    {
        Dictionary<string, string> errors = new();

        if (command is null)
        {
            errors.Add(key: nameof(command), value: "Command is required");

            return errors;
        }

        if (command.Preference.UserId == Guid.Empty)
        {
            errors.Add(key: nameof(command.Preference.UserId), value: "User id cannot be empty");
        }

        return errors;
    }
}