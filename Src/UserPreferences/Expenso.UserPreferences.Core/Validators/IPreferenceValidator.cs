using Expenso.UserPreferences.Core.DTO.UpdateUserPreferences;
using Expenso.UserPreferences.Core.Models;

namespace Expenso.UserPreferences.Core.Validators;

internal interface IPreferenceValidator
{
    Task ValidateCreateAsync(Guid userId, CancellationToken cancellationToken);

    Task<Preference> ValidateUpdateAsync(Guid preferenceIdOrUserId, UpdatePreferenceDto preferenceDto,
        CancellationToken cancellationToken);
}