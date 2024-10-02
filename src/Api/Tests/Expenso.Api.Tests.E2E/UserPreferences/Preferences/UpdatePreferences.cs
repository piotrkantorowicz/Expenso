using Expenso.Api.Tests.E2E.TestData.Preferences;
using Expenso.UserPreferences.Core.Application.Preferences.Write.Commands.UpdatePreference.DTO.Requests;

namespace Expenso.Api.Tests.E2E.UserPreferences.Preferences;

internal sealed class UpdatePreferences : PreferencesTestBase
{
    [Test]
    public async Task Should_ReturnExpectedResult()
    {
        // Arrange
        Guid? preferenceId = PreferencesDataInitializer.PreferenceIds[index: 1];
        _httpClient.SetFakeBearerToken(token: _claims);
        string requestPath = $"user-preferences/preferences/{preferenceId}";

        // Act
        HttpResponseMessage response = await _httpClient.PutAsJsonAsync(requestUri: requestPath,
            value: new UpdatePreferenceRequest(FinancePreference: new UpdatePreferenceRequestFinancePreference(
                    AllowAddFinancePlanSubOwners: true,
                    MaxNumberOfSubFinancePlanSubOwners: 5, AllowAddFinancePlanReviewers: true,
                    MaxNumberOfFinancePlanReviewers: 10),
                NotificationPreference: new UpdatePreferenceRequestNotificationPreference(
                    SendFinanceReportEnabled: true, SendFinanceReportInterval: 1),
                GeneralPreference: new UpdatePreferenceRequestGeneralPreference(UseDarkMode: true)));

        // Assert
        AssertResponseNoContent(response: response);
    }

    [Test]
    public async Task Should_Return401_When_NoAccessTokenProvided()
    {
        // Arrange
        Guid? preferenceId = PreferencesDataInitializer.PreferenceIds[index: 1];

        // Act
        HttpResponseMessage response =
            await _httpClient.PutAsync(requestUri: $"user-preferences/preferences/{preferenceId}", content: null);

        // Assert
        AssertResponseUnauthroised(response: response);
    }
}