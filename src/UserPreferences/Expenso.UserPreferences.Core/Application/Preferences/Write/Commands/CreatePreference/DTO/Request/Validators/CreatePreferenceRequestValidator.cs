using Expenso.UserPreferences.Shared.DTO.API.CreatePreference.Request;

using FluentValidation;

namespace Expenso.UserPreferences.Core.Application.Preferences.Write.Commands.CreatePreference.DTO.Request.Validators;

internal sealed class CreatePreferenceRequestValidator : AbstractValidator<CreatePreferenceRequest>
{
    public CreatePreferenceRequestValidator()
    {
        RuleFor(expression: x => x.UserId).NotEmpty().WithMessage(errorMessage: "User ID cannot be empty.");
    }
}