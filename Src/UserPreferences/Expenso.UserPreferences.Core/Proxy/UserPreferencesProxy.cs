using Expenso.UserPreferences.Core.Services;
using Expenso.UserPreferences.Proxy;
using Expenso.UserPreferences.Proxy.Contracts.GetUserPreferences;

namespace Expenso.UserPreferences.Core.Proxy;

internal sealed class UserPreferencesProxy(IPreferencesService preferencesService) : IUserPreferencesProxy
{
    private readonly IPreferencesService _preferencesService =
        preferencesService ?? throw new ArgumentNullException(nameof(preferencesService));

    public async Task<PreferenceContract> GetUserPreferencesAsync(Guid userId, CancellationToken cancellationToken)
    {
        return await _preferencesService.GetPreferencesInternalAsync(userId, cancellationToken);
    }

    public async Task CreatePreferencesAsync(Guid userId, CancellationToken cancellationToken)
    {
        await _preferencesService.CreatePreferencesInternalAsync(userId, cancellationToken);
    }
}