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
                    command: new CreatePreferenceCommand(MessageContext: messageContextFactory.Current(),
                        Preference: new CreatePreferenceRequest(UserId: userId)), cancellationToken: cancellationToken);

            if (preference is not null)
            {
                PreferenceIds?.Add(item: preference.PreferenceId);
            }
        }

        await commandDispatcher.SendAsync(
            command: new UpdatePreferenceCommand(MessageContext: messageContextFactory.Current(),
                PreferenceOrUserId: UserDataInitializer.UserIds[index: 0],
                Preference: new UpdatePreferenceRequest(
                    FinancePreference: new UpdatePreferenceRequest_FinancePreference(AllowAddFinancePlanSubOwners: true,
                        MaxNumberOfSubFinancePlanSubOwners: 3, AllowAddFinancePlanReviewers: true,
                        MaxNumberOfFinancePlanReviewers: 5),
                    NotificationPreference: new UpdatePreferenceRequest_NotificationPreference(
                        SendFinanceReportEnabled: true, SendFinanceReportInterval: 7),
                    GeneralPreference: new UpdatePreferenceRequest_GeneralPreference(UseDarkMode: true))),
            cancellationToken: cancellationToken);
    }
}