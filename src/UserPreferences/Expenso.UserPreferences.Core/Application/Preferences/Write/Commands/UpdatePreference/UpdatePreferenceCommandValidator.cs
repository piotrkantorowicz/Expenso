using Expenso.Shared.Commands.Validation.Validators;
using Expenso.UserPreferences.Core.Application.Preferences.Write.Commands.UpdatePreference.DTO.Request.Validators;

using FluentValidation;

namespace Expenso.UserPreferences.Core.Application.Preferences.Write.Commands.UpdatePreference;

internal sealed class UpdatePreferenceCommandValidator : CommandValidator<UpdatePreferenceCommand>
{
    public UpdatePreferenceCommandValidator(MessageContextValidator messageContextValidator,
        UpdatePreferenceRequestValidator updatePreferenceCommandValidator) : base(
        messageContextValidator: messageContextValidator)
    {
        ArgumentNullException.ThrowIfNull(argument: updatePreferenceCommandValidator,
            paramName: nameof(updatePreferenceCommandValidator));

        RuleFor(expression: x => x.PreferenceId)
            .NotEmpty()
            .WithMessage(errorMessage: "The preference ID must not be empty.");

        RuleFor(expression: x => x.Payload!)
            .NotNull()
            .WithMessage(errorMessage: "The command payload must not be null.")
            .DependentRules(action: () =>
                RuleFor(expression: x => x.Payload!).SetValidator(validator: updatePreferenceCommandValidator));
    }
}