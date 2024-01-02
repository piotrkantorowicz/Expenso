using Expenso.UserPreferences.Proxy.Contracts.GetUserPreferences;

namespace Expenso.UserPreferences.Proxy;

public interface IUserPreferencesProxy
{
    Task<PreferenceContract> GetUserPreferencesAsync(Guid userId, CancellationToken cancellationToken);

    Task CreatePreferencesAsync(Guid userId, CancellationToken cancellationToken);
}