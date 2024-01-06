using Expenso.UserPreferences.Proxy;
using Expenso.UserPreferences.Proxy.Contracts.GetUserPreferences;

namespace Expenso.Api.Tests.E2E.TestData;

internal static class PreferencesDataProvider
{
    public static readonly IList<PreferenceContract>? Preferences = new List<PreferenceContract>();

    public static async Task Initialize(IUserPreferencesProxy preferencesProxy, CancellationToken cancellationToken)
    {
        foreach (Guid userId in UsersDataProvider.UserIds)
        {
            PreferenceContract preferenceContract =
                await preferencesProxy.CreatePreferencesAsync(userId, cancellationToken);

            Preferences?.Add(preferenceContract);
        }
    }
}