using Expenso.Shared.Commands.Dispatchers;
using Expenso.Shared.Queries.Dispatchers;
using Expenso.Shared.System.Types.Messages.Interfaces;
using Expenso.UserPreferences.Core.Application.Preferences.Read.Queries.GetPreference;
using Expenso.UserPreferences.Core.Application.Preferences.Write.Commands.CreatePreference;
using Expenso.UserPreferences.Proxy;
using Expenso.UserPreferences.Proxy.DTO.API.CreatePreference.Request;
using Expenso.UserPreferences.Proxy.DTO.API.CreatePreference.Response;
using Expenso.UserPreferences.Proxy.DTO.API.GetPreference.Response;

namespace Expenso.UserPreferences.Core.Application.Proxy;

internal sealed class UserPreferencesProxy(
    ICommandDispatcher commandDispatcher,
    IQueryDispatcher queryDispatcher,
    IMessageContextFactory messageContextFactory) : IUserPreferencesProxy
{
    private readonly ICommandDispatcher _commandDispatcher =
        commandDispatcher ?? throw new ArgumentNullException(paramName: nameof(commandDispatcher));

    private readonly IMessageContextFactory _messageContextFactory = messageContextFactory ??
                                                                     throw new ArgumentNullException(
                                                                         paramName: nameof(messageContextFactory));

    private readonly IQueryDispatcher _queryDispatcher =
        queryDispatcher ?? throw new ArgumentNullException(paramName: nameof(queryDispatcher));

    public async Task<GetPreferenceResponse?> GetUserPreferencesAsync(Guid userId, bool includeFinancePreferences,
        bool includeNotificationPreferences, bool includeGeneralPreferences,
        CancellationToken cancellationToken = default)
    {
        return await _queryDispatcher.QueryAsync(
            query: new GetPreferenceQuery(MessageContext: _messageContextFactory.Current(), UserId: userId,
                IncludeFinancePreferences: includeFinancePreferences,
                IncludeNotificationPreferences: includeNotificationPreferences,
                IncludeGeneralPreferences: includeGeneralPreferences), cancellationToken: cancellationToken);
    }

    public async Task<CreatePreferenceResponse?> CreatePreferencesAsync(CreatePreferenceRequest createPreferenceRequest,
        CancellationToken cancellationToken = default)
    {
        return await _commandDispatcher.SendAsync<CreatePreferenceCommand, CreatePreferenceResponse>(
            command: new CreatePreferenceCommand(MessageContext: _messageContextFactory.Current(),
                Preference: createPreferenceRequest), cancellationToken: cancellationToken);
    }
}