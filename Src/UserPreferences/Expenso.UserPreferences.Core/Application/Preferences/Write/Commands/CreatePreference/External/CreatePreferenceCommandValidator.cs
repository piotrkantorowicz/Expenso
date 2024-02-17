using Expenso.Shared.Commands.Validation;

namespace Expenso.UserPreferences.Core.Application.Preferences.Write.Commands.CreatePreference.External;

internal sealed class CreatePreferenceCommandValidator : ICommandValidator<CreatePreferenceCommand>
{
    public IDictionary<string, string> Validate(CreatePreferenceCommand command)
    {
        Dictionary<string, string> errors = new();

        if (command.Preference.UserId == Guid.Empty)
        {
            errors.Add(nameof(command.Preference.UserId), "User id cannot be empty");
        }

        return errors;
    }
}