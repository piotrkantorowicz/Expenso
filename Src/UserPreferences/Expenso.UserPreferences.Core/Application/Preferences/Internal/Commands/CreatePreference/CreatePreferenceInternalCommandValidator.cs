using Expenso.Shared.Commands.Validation;

namespace Expenso.UserPreferences.Core.Application.Preferences.Internal.Commands.CreatePreference;

internal sealed class CreatePreferenceInternalCommandValidator : ICommandValidator<CreatePreferenceInternalCommand>
{
    public IDictionary<string, string> Validate(CreatePreferenceInternalCommand command)
    {
        Dictionary<string, string> errors = new();

        if (command.Preference.UserId == Guid.Empty)
        {
            errors.Add(nameof(command.Preference.UserId), "User id cannot be empty.");
        }

        return errors;
    }
}