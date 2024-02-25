using Expenso.UserPreferences.Proxy;
using Expenso.UserPreferences.Proxy.DTO.API.CreatePreference.Response;

namespace Expenso.Api.Tests.E2E.TestData;

internal static class PreferencesDataProvider
{
    public static readonly IList<Guid> PreferenceIds = new List<Guid>();

    public static async Task Initialize(IUserPreferencesProxy preferencesProxy, CancellationToken cancellationToken)
    {
        foreach (Guid userId in UsersDataProvider.UserIds)
        {
            CreatePreferenceResponse? preference =
                await preferencesProxy.CreatePreferencesAsync(userId, cancellationToken);

            if (preference is not null)
            {
                PreferenceIds?.Add(preference.PreferenceId);
            }
        }
    }
}