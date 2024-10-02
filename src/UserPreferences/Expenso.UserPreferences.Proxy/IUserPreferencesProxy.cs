using Expenso.UserPreferences.Proxy.DTO.API.CreatePreference.Requests;
using Expenso.UserPreferences.Proxy.DTO.API.CreatePreference.Responses;
using Expenso.UserPreferences.Proxy.DTO.API.GetPreference.Responses;

namespace Expenso.UserPreferences.Proxy;

public interface IUserPreferencesProxy
{
    Task<GetPreferenceResponse?> GetUserPreferencesAsync(Guid userId, bool includeFinancePreferences,
        bool includeNotificationPreferences, bool includeGeneralPreferences,
        CancellationToken cancellationToken = default);

    Task<CreatePreferenceResponse?> CreatePreferencesAsync(CreatePreferenceRequest createPreferenceRequest,
        CancellationToken cancellationToken = default);
}