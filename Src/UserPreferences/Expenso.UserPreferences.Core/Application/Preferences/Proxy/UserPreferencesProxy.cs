using Expenso.Shared.Commands.Dispatchers;
using Expenso.Shared.Queries.Dispatchers;
using Expenso.UserPreferences.Core.Application.Preferences.Internal.Commands.CreatePreference;
using Expenso.UserPreferences.Core.Application.Preferences.Internal.Queries.GetPreference;
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

    public async Task<GetPreferenceInternalResponse?> GetUserPreferencesAsync(Guid userId,
        CancellationToken cancellationToken = default)
    {
        return await _queryDispatcher.QueryAsync(new GetPreferenceInternalQuery(userId), cancellationToken);
    }

    public async Task<CreatePreferenceInternalResponse?> CreatePreferencesAsync(Guid userId,
        CancellationToken cancellationToken = default)
    {
        return await _commandDispatcher.SendAsync<CreatePreferenceInternalCommand, CreatePreferenceInternalResponse>(
            new CreatePreferenceInternalCommand(new CreatePreferenceInternalRequest(userId)), cancellationToken);
    }
}