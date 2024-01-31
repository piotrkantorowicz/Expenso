using Expenso.UserPreferences.Proxy.DTO.API.CreatePreference.Response;
using Expenso.UserPreferences.Proxy.DTO.API.GetPreference.Request;

namespace Expenso.UserPreferences.Proxy;

public interface IUserPreferencesProxy
{
    Task<GetPreferenceInternalResponse?> GetUserPreferencesAsync(Guid userId,
        CancellationToken cancellationToken = default);

    Task<CreatePreferenceInternalResponse?> CreatePreferencesAsync(Guid userId,
        CancellationToken cancellationToken = default);
}