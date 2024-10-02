using Expenso.Shared.Commands.Dispatchers;
using Expenso.Shared.Queries.Dispatchers;
using Expenso.Shared.System.Types.Messages.Interfaces;
using Expenso.UserPreferences.Core.Application.Preferences.Read.Queries.GetPreference;
using Expenso.UserPreferences.Core.Application.Preferences.Write.Commands.CreatePreference;
using Expenso.UserPreferences.Proxy;
using Expenso.UserPreferences.Proxy.DTO.API.CreatePreference.Requests;
using Expenso.UserPreferences.Proxy.DTO.API.CreatePreference.Responses;
using Expenso.UserPreferences.Proxy.DTO.API.GetPreference.Responses;

namespace Expenso.UserPreferences.Core.Application.Proxy;

internal sealed class UserPreferencesProxy : IUserPreferencesProxy
{
    private readonly ICommandDispatcher _commandDispatcher;
    private readonly IMessageContextFactory _messageContextFactory;
    private readonly IQueryDispatcher _queryDispatcher;

    public UserPreferencesProxy(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher,
        IMessageContextFactory messageContextFactory)
    {
        _commandDispatcher = commandDispatcher ?? throw new ArgumentNullException(paramName: nameof(commandDispatcher));

        _messageContextFactory = messageContextFactory ??
                                 throw new ArgumentNullException(paramName: nameof(messageContextFactory));

        _queryDispatcher = queryDispatcher ?? throw new ArgumentNullException(paramName: nameof(queryDispatcher));
    }

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