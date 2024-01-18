using Expenso.UserPreferences.Core.Application.DTO.UpdateUserPreferences;
using Expenso.UserPreferences.Core.Domain.Preferences.Model;

namespace Expenso.UserPreferences.Core.Application.Validators;

internal interface IPreferenceValidator
{
    Task ValidateCreateAsync(Guid userId, CancellationToken cancellationToken);

    Task<Preference> ValidateUpdateAsync(Guid preferenceIdOrUserId, UpdatePreferenceDto preferenceDto,
        CancellationToken cancellationToken);
}