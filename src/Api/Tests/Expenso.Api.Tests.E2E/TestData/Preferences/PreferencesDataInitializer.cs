using Expenso.Api.Tests.E2E.TestData.IAM;
using Expenso.Shared.Commands.Dispatchers;
using Expenso.Shared.System.Types.Messages.Interfaces;
using Expenso.UserPreferences.Core.Application.Preferences.Write.Commands.CreatePreference;
using Expenso.UserPreferences.Core.Application.Preferences.Write.Commands.UpdatePreference;
using Expenso.UserPreferences.Core.Application.Preferences.Write.Commands.UpdatePreference.DTO.Requests;
using Expenso.UserPreferences.Proxy.DTO.API.CreatePreference.Requests;
using Expenso.UserPreferences.Proxy.DTO.API.CreatePreference.Responses;

namespace Expenso.Api.Tests.E2E.TestData.Preferences;

internal static class PreferencesDataInitializer
{
    public static readonly IList<Guid> PreferenceIds = new List<Guid>();

    public static async Task InitializeAsync(ICommandDispatcher commandDispatcher,
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
                Preference: new UpdatePreferenceRequest(FinancePreference: new UpdatePreferenceRequestFinancePreference(
                        AllowAddFinancePlanSubOwners: true,
                        MaxNumberOfSubFinancePlanSubOwners: 3, AllowAddFinancePlanReviewers: true,
                        MaxNumberOfFinancePlanReviewers: 5),
                    NotificationPreference: new UpdatePreferenceRequestNotificationPreference(
                        SendFinanceReportEnabled: true, SendFinanceReportInterval: 7),
                    GeneralPreference: new UpdatePreferenceRequestGeneralPreference(UseDarkMode: true))),
            cancellationToken: cancellationToken);
    }
}