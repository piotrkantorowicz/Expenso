using Expenso.Api.Tests.E2E.TestData.IAM;
using Expenso.Shared.Commands.Dispatchers;
using Expenso.Shared.System.Modules.Constants;
using Expenso.Shared.System.Types.Clock;
using Expenso.Shared.System.Types.Messages;
using Expenso.UserPreferences.Core.Application.Preferences.Write.Commands.CreatePreference;
using Expenso.UserPreferences.Core.Application.Preferences.Write.Commands.UpdatePreference;
using Expenso.UserPreferences.Core.Application.Preferences.Write.Commands.UpdatePreference.DTO.Request;
using Expenso.UserPreferences.Shared.DTO.API.CreatePreference.Request;
using Expenso.UserPreferences.Shared.DTO.API.CreatePreference.Response;

namespace Expenso.Api.Tests.E2E.TestData.Preferences;

internal static class PreferencesDataInitializer
{
    public static readonly IList<Guid> PreferenceIds = new List<Guid>();

    public static async Task InitializeAsync(ICommandDispatcher commandDispatcher, IClock clock,
        CancellationToken cancellationToken)
    {
        Guid correlationId = Guid.NewGuid();

        foreach (Guid userId in UserDataInitializer.UserIds)
        {
            CreatePreferenceResponse? preference =
                await commandDispatcher.SendAsync<CreatePreferenceCommand, CreatePreferenceResponse>(
                    command: new CreatePreferenceCommand(MessageContext: new MessageContext(messageId: Guid.NewGuid(),
                            correlationId: correlationId,
                            requestedBy: TestClient.ClientId, timestamp: clock.UtcNow,
                            module: ModuleNames.UserPreferencesModule),
                        Payload: new CreatePreferenceRequest(UserId: userId)), cancellationToken: cancellationToken);

            PreferenceIds.Add(item: preference!.PreferenceId);
        }

        await commandDispatcher.SendAsync(command: new UpdatePreferenceCommand(
                MessageContext: new MessageContext(messageId: Guid.NewGuid(), correlationId: correlationId,
                    requestedBy: TestClient.ClientId, timestamp: clock.UtcNow,
                    module: ModuleNames.UserPreferencesModule), PreferenceId: PreferenceIds[index: 0],
                Payload: new UpdatePreferenceRequest(
                    FinancePreference: new UpdatePreferenceRequestFinancePreference(AllowAddFinancePlanSubOwners: true,
                        MaxNumberOfSubFinancePlanSubOwners: 3, AllowAddFinancePlanReviewers: true,
                        MaxNumberOfFinancePlanReviewers: 5),
                    NotificationPreference: new UpdatePreferenceRequestNotificationPreference(
                        SendFinanceReportEnabled: true, SendFinanceReportInterval: 7),
                    GeneralPreference: new UpdatePreferenceRequestGeneralPreference(UseDarkMode: true))),
            cancellationToken: cancellationToken);
    }
}