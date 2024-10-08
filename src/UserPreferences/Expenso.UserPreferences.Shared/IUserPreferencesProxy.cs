using Expenso.UserPreferences.Shared.DTO.API.CreatePreference.Request;
using Expenso.UserPreferences.Shared.DTO.API.CreatePreference.Response;
using Expenso.UserPreferences.Shared.DTO.API.GetPreference.Request;
using Expenso.UserPreferences.Shared.DTO.API.GetPreference.Response;

namespace Expenso.UserPreferences.Shared;

public interface IUserPreferencesProxy
{
    Task<GetPreferencesResponse?> GetPreferences(GetPreferencesRequest getPreferenceRequest,
        CancellationToken cancellationToken = default);

    Task<CreatePreferenceResponse?> CreatePreferencesAsync(CreatePreferenceRequest request,
        CancellationToken cancellationToken = default);
}