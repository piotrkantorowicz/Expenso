using Expenso.Shared.Commands.Validation;
using Expenso.Shared.Commands.Validation.Helpers;
using Expenso.Shared.Commands.Validation.Utils;

namespace Expenso.UserPreferences.Core.Application.Preferences.Write.Commands.CreatePreference;

internal sealed class CreatePreferenceCommandValidator : ICommandValidator<CreatePreferenceCommand>
{
    private readonly IReadOnlyDictionary<string, CommandValidationRule<CreatePreferenceCommand>[]> _validationMetadata =
        new Dictionary<string, CommandValidationRule<CreatePreferenceCommand>[]>
        {
            {
                ValidationUtils.Command, [
                    new CommandValidationRule<CreatePreferenceCommand>(validationFailedFunc: command => command is null,
                        createMessageFunc: _ => "Command is required", validationType: ValidationTypes.Required,
                        value: true)
                ]
            },
            {
                nameof(CreatePreferenceCommand.Preference.UserId), [
                    new CommandValidationRule<CreatePreferenceCommand>(
                        validationFailedFunc: command => command?.Preference.UserId == Guid.Empty,
                        createMessageFunc: _ => "User id cannot be empty", validationType: ValidationTypes.Required,
                        value: true)
                ]
            }
        };

    public IReadOnlyDictionary<string, CommandValidationRule<CreatePreferenceCommand>[]> GetValidationMetadata()
    {
        return _validationMetadata.ToDictionary(keySelector: kvp => kvp.Key, elementSelector: kvp => kvp.Value);
    }

    public IDictionary<string, string> Validate(CreatePreferenceCommand? command)
    {
        Dictionary<string, string> errors = new();
        ValidationHelper.ValidateAll(errors: errors, validationMetadata: _validationMetadata, command: command);

        return errors;
    }
}