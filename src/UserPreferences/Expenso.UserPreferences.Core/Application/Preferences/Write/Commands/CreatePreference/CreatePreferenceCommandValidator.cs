using Expenso.Shared.Commands.Validation;
using Expenso.UserPreferences.Core.Application.Preferences.Write.Commands.CreatePreference.DTO.Request.Validators;

using FluentValidation;

namespace Expenso.UserPreferences.Core.Application.Preferences.Write.Commands.CreatePreference;

internal sealed class CreatePreferenceCommandValidator : CommandValidator<CreatePreferenceCommand>
{
    public CreatePreferenceCommandValidator(MessageContextValidator messageContextValidator,
        CreatePreferenceRequestValidator preferenceRequestValidator) : base(
        messageContextValidator: messageContextValidator)
    {
        ArgumentNullException.ThrowIfNull(argument: preferenceRequestValidator,
            paramName: nameof(preferenceRequestValidator));

        RuleFor(expression: x => x.Payload).NotNull();

        When(predicate: x => x.Payload != null, action: () =>
        {
            RuleFor(expression: x => x.Payload).SetValidator(validator: preferenceRequestValidator!);
        });
    }
}