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
        RuleFor(expression: x => x.Payload).NotNull();
        RuleFor(expression: x => x.PreferenceId).NotEmpty().WithMessage(errorMessage: "Preference ID cannot be empty.");

        When(predicate: x => x.Payload != null, action: () =>
        {
            RuleFor(expression: x => x.Payload).SetValidator(validator: updatePreferenceCommandValidator!);
        });
    }
}