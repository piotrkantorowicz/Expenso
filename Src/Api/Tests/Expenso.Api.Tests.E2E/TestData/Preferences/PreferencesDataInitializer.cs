using Expenso.Api.Tests.E2E.TestData.IAM;
using Expenso.Shared.Commands.Dispatchers;
using Expenso.Shared.System.Types.Messages.Interfaces;
using Expenso.UserPreferences.Core.Application.Preferences.Write.Commands.CreatePreference;
using Expenso.UserPreferences.Core.Application.Preferences.Write.Commands.UpdatePreference;
using Expenso.UserPreferences.Core.Application.Preferences.Write.Commands.UpdatePreference.DTO.Request;
using Expenso.UserPreferences.Proxy.DTO.API.CreatePreference.Request;
using Expenso.UserPreferences.Proxy.DTO.API.CreatePreference.Response;

namespace Expenso.Api.Tests.E2E.TestData.Preferences;

internal static class PreferencesDataInitializer
{
    public static readonly IList<Guid> PreferenceIds = new List<Guid>();

    public static async Task Initialize(ICommandDispatcher commandDispatcher,
        IMessageContextFactory messageContextFactory, CancellationToken cancellationToken)
    {
        foreach (Guid userId in UserDataInitializer.UserIds)
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

        await commandDispatcher.SendAsync(new UpdatePreferenceCommand(messageContextFactory.Current(),
            UserDataInitializer.UserIds[0],
                new UpdatePreferenceRequest(new UpdatePreferenceRequest_FinancePreference(true, 3, true, 5),
                    new UpdatePreferenceRequest_NotificationPreference(true, 7),
                    new UpdatePreferenceRequest_GeneralPreference(true))), cancellationToken);
    }
}