using Expenso.UserPreferences.Core.DTO.GetUserPreferences;
using Expenso.UserPreferences.Core.DTO.UpdateUserPreferences;
using Expenso.UserPreferences.Proxy.Contracts.GetUserPreferences;

namespace Expenso.UserPreferences.Core.Services;

public interface IPreferencesService
{
    Task<PreferenceDto> GetPreferencesForCurrentUserAsync(CancellationToken cancellationToken);

    Task<PreferenceDto> GetPreferencesAsync(Guid userId, CancellationToken cancellationToken);

    Task<PreferenceContract> GetPreferencesInternalAsync(Guid userId, CancellationToken cancellationToken);

    Task CreatePreferencesAsync(Guid userId, CancellationToken cancellationToken);

    Task CreatePreferencesInternalAsync(Guid userId, CancellationToken cancellationToken);

    Task UpdatePreferencesAsync(Guid preferenceIdOrUserId, UpdatePreferenceDto preferenceDto,
        CancellationToken cancellationToken);
}