using Expenso.UserPreferences.Proxy.DTO.API.CreatePreference.Request;
using Expenso.UserPreferences.Proxy.DTO.API.CreatePreference.Response;
using Expenso.UserPreferences.Proxy.DTO.API.GetPreference.Response;

namespace Expenso.UserPreferences.Proxy;

public interface IUserPreferencesProxy
{
    Task<GetPreferenceResponse?> GetUserPreferencesAsync(Guid userId, bool includeFinancePreferences,
        bool includeNotificationPreferences, bool includeGeneralPreferences,
        CancellationToken cancellationToken = default);

    Task<CreatePreferenceResponse?> CreatePreferencesAsync(CreatePreferenceRequest createPreferenceRequest,
        CancellationToken cancellationToken = default);
}