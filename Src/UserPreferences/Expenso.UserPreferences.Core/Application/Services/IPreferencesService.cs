using Expenso.UserPreferences.Core.Application.DTO.GetUserPreferences;
using Expenso.UserPreferences.Core.Application.DTO.UpdateUserPreferences;
using Expenso.UserPreferences.Proxy.Contracts.GetUserPreferences;

namespace Expenso.UserPreferences.Core.Application.Services;

public interface IPreferencesService
{
    Task<PreferenceDto> GetPreferences(Guid preferenceId, CancellationToken cancellationToken);
    
    Task<PreferenceDto> GetPreferencesForCurrentUserAsync(CancellationToken cancellationToken);

    Task<PreferenceDto> GetPreferencesForUserAsync(Guid userId, CancellationToken cancellationToken);

    Task<PreferenceContract> GetPreferencesForUserInternalAsync(Guid userId, CancellationToken cancellationToken);

    Task<PreferenceDto> CreatePreferencesAsync(Guid userId, CancellationToken cancellationToken);

    Task<PreferenceContract> CreatePreferencesInternalAsync(Guid userId, CancellationToken cancellationToken);

    Task UpdatePreferencesAsync(Guid preferenceIdOrUserId, UpdatePreferenceDto preferenceDto,
        CancellationToken cancellationToken);
}