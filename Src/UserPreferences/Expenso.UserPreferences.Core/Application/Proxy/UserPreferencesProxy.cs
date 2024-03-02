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
        commandDispatcher ?? throw new ArgumentNullException(nameof(commandDispatcher));

    private readonly IMessageContextFactory _messageContextFactory =
        messageContextFactory ?? throw new ArgumentNullException(nameof(messageContextFactory));

    private readonly IQueryDispatcher _queryDispatcher =
        queryDispatcher ?? throw new ArgumentNullException(nameof(queryDispatcher));

    public async Task<GetPreferenceResponse?> GetUserPreferencesAsync(Guid userId, bool includeFinancePreferences,
        bool includeNotificationPreferences, bool includeGeneralPreferences,
        CancellationToken cancellationToken = default)
    {
        return await _queryDispatcher.QueryAsync(
            new GetPreferenceQuery(_messageContextFactory.Current(), UserId: userId,
                IncludeFinancePreferences: includeFinancePreferences,
                IncludeNotificationPreferences: includeNotificationPreferences,
                IncludeGeneralPreferences: includeGeneralPreferences), cancellationToken);
    }

    public async Task<CreatePreferenceResponse?> CreatePreferencesAsync(Guid userId,
        CancellationToken cancellationToken = default)
    {
        return await _commandDispatcher.SendAsync<CreatePreferenceCommand, CreatePreferenceResponse>(
            new CreatePreferenceCommand(_messageContextFactory.Current(), new CreatePreferenceRequest(userId)),
            cancellationToken);
    }
}