using Expenso.Shared.Commands.Dispatchers;
using Expenso.Shared.Queries.Dispatchers;
using Expenso.UserPreferences.Core.Application.Preferences.Read.Queries.GetPreference.External;
using Expenso.UserPreferences.Core.Application.Preferences.Write.Commands.CreatePreference.External;
using Expenso.UserPreferences.Proxy;
using Expenso.UserPreferences.Proxy.DTO.API.CreatePreference.Request;
using Expenso.UserPreferences.Proxy.DTO.API.CreatePreference.Response;
using Expenso.UserPreferences.Proxy.DTO.API.GetPreference.Request;

namespace Expenso.UserPreferences.Core.Application.Preferences.Proxy;

internal sealed class UserPreferencesProxy(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher)
    : IUserPreferencesProxy
{
    private readonly ICommandDispatcher _commandDispatcher =
        commandDispatcher ?? throw new ArgumentNullException(nameof(commandDispatcher));

    private readonly IQueryDispatcher _queryDispatcher =
        queryDispatcher ?? throw new ArgumentNullException(nameof(queryDispatcher));

    public async Task<GetPreferenceResponse?> GetUserPreferencesAsync(Guid userId, bool includeFinancePreferences,
        bool includeNotificationPreferences, bool includeGeneralPreferences,
        CancellationToken cancellationToken = default)
    {
        return await _queryDispatcher.QueryAsync(
            new GetPreferenceQuery(userId, includeFinancePreferences, includeNotificationPreferences,
                includeGeneralPreferences), cancellationToken);
    }

    public async Task<CreatePreferenceResponse?> CreatePreferencesAsync(Guid userId,
        CancellationToken cancellationToken = default)
    {
        return await _commandDispatcher.SendAsync<CreatePreferenceCommand, CreatePreferenceResponse>(
            new CreatePreferenceCommand(new CreatePreferenceRequest(userId)), cancellationToken);
    }
}