using Expenso.UserPreferences.Shared.DTO.API.UpdatePreference;

using FluentValidation;

namespace Expenso.UserPreferences.Core.Application.Preferences.Write.Commands.UpdatePreference.DTO.Request.Validators;

internal sealed class
    UpdatePreferenceRequestGeneralPreferenceValidator : AbstractValidator<UpdatePreferenceRequestGeneralPreference>;