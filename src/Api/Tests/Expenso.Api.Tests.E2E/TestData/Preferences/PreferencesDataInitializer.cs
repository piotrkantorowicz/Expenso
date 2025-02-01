using Expenso.Api.Tests.E2E.TestData.IAM;
using Expenso.Shared.System.Modules.Constants;
using Expenso.Shared.System.Time;
using Expenso.Shared.System.Types.Messages;
using Expenso.UserPreferences.Shared;
using Expenso.UserPreferences.Shared.DTO.API.CreatePreference.Request;
using Expenso.UserPreferences.Shared.DTO.API.CreatePreference.Response;
using Expenso.UserPreferences.Shared.DTO.API.UpdatePreference;

namespace Expenso.Api.Tests.E2E.TestData.Preferences;

internal static class PreferencesDataInitializer
{
    public static readonly IList<Guid> PreferenceIds = new List<Guid>();

    public static async Task InitializeAsync(IClock clock, IUserPreferencesProxy userPreferencesProxy,
        CancellationToken cancellationToken)
    {
        Guid correlationId = Guid.CreateVersion7();

        foreach (Guid userId in UserDataInitializer.UserIds)
        {
            CreatePreferenceResponse? preference = await userPreferencesProxy.CreatePreferencesAsync(
                request: new CreatePreferenceRequest(UserId: userId),
                messageContext: new MessageContext(messageId: Guid.CreateVersion7(), correlationId: correlationId,
                    requestedBy: TestClient.ClientId, timestamp: clock.UtcNow,
                    module: ModuleNames.UserPreferencesModule), cancellationToken: cancellationToken);

            PreferenceIds.Add(item: preference!.PreferenceId);
        }

        UpdatePreferenceRequest updatePreferenceRequest = new(
            FinancePreference: new UpdatePreferenceRequestFinancePreference(AllowAddFinancePlanSubOwners: true,
                MaxNumberOfSubFinancePlanSubOwners: 3, AllowAddFinancePlanReviewers: true,
                MaxNumberOfFinancePlanReviewers: 5),
            NotificationPreference: new UpdatePreferenceRequestNotificationPreference(SendFinanceReportEnabled: true,
                SendFinanceReportInterval: 7),
            GeneralPreference: new UpdatePreferenceRequestGeneralPreference(UseDarkMode: true));

        await userPreferencesProxy.UpdatePreferencesAsync(preferenceId: PreferenceIds[index: 0],
            request: updatePreferenceRequest,
            messageContext: new MessageContext(messageId: Guid.CreateVersion7(), correlationId: correlationId,
                requestedBy: TestClient.ClientId, timestamp: clock.UtcNow, module: ModuleNames.UserPreferencesModule),
            cancellationToken: cancellationToken);
    }
}