using Expenso.Shared.Commands.Dispatchers;
using Expenso.Shared.System.Types.Messages.Interfaces;
using Expenso.UserPreferences.Core.Application.Preferences.Write.Commands.CreatePreference;
using Expenso.UserPreferences.Core.Application.Preferences.Write.Commands.UpdatePreference;
using Expenso.UserPreferences.Core.Application.Preferences.Write.Commands.UpdatePreference.DTO.Request;
using Expenso.UserPreferences.Proxy.DTO.API.CreatePreference.Request;
using Expenso.UserPreferences.Proxy.DTO.API.CreatePreference.Response;

namespace Expenso.Api.Tests.E2E.TestData.Preferences;

internal static class PreferencesDataProvider

{
    public static readonly IList<Guid> UserIds =
    [
        new Guid("1eee2753-4ebd-40e8-af35-f09bbce44bf9"),
        new Guid("3318e89e-fe27-453b-b9cb-3edce39ee187"),
        new Guid("41e0197a-014f-419a-9521-d0946e88818d"),
        new Guid("a8033772-3f5a-4127-8d23-de01aaf4f5d9"),
        new Guid("b754111e-39b4-4d54-92ee-128f3b456d0a")
    ];

    public static readonly IList<Guid> PreferenceIds = new List<Guid>();

    public static async Task Initialize(ICommandDispatcher commandDispatcher,
        IMessageContextFactory messageContextFactory, CancellationToken cancellationToken)
    {
        foreach (Guid userId in UserIds)
        {
            CreatePreferenceResponse? preference =
                await commandDispatcher.SendAsync<CreatePreferenceCommand, CreatePreferenceResponse>(
                    new CreatePreferenceCommand(messageContextFactory.Current(), new CreatePreferenceRequest(userId)),
                    cancellationToken);

            if (preference is not null)
            {
                PreferenceIds?.Add(preference.PreferenceId);
            }
        }

        await commandDispatcher.SendAsync(
            new UpdatePreferenceCommand(messageContextFactory.Current(), UserIds[0],
                new UpdatePreferenceRequest(new UpdatePreferenceRequest_FinancePreference(true, 3, true, 5),
                    new UpdatePreferenceRequest_NotificationPreference(true, 7),
                    new UpdatePreferenceRequest_GeneralPreference(true))), cancellationToken);
    }
}