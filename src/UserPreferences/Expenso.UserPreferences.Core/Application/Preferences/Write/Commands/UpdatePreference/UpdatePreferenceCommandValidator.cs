using Expenso.Shared.Commands.Validation;
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

        RuleFor(expression: x => x.Payload)
            .NotNull()
            .WithMessage(errorMessage: "The command payload must not be null.")
            .SetValidator(validator: updatePreferenceCommandValidator!);
    }
}