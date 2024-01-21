using Expenso.Shared.Commands.Validations;

namespace Expenso.UserPreferences.Core.Application.Preferences.Commands.CreatePreference;

internal sealed class CreatePreferenceCommandValidator : ICommandValidator<CreatePreferenceCommand>
{
    public IDictionary<string, string> Validate(CreatePreferenceCommand command)
    {
        Dictionary<string, string> errors = new();

        if (command.Preference.UserId == Guid.Empty)
        {
            errors.Add(nameof(command.Preference.UserId), "User id cannot be empty.");
        }

        return errors;
    }
}