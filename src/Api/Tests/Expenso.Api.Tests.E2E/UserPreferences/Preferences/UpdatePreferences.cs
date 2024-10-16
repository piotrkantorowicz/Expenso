using Expenso.Api.Tests.E2E.TestData.Preferences;
using Expenso.UserPreferences.Core.Application.Preferences.Write.Commands.UpdatePreference.DTO.Request;

namespace Expenso.Api.Tests.E2E.UserPreferences.Preferences;

[TestFixture]
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
            value: new UpdatePreferenceRequest(
                FinancePreference: new UpdatePreferenceRequest_FinancePreference(AllowAddFinancePlanSubOwners: true,
                    MaxNumberOfSubFinancePlanSubOwners: 5, AllowAddFinancePlanReviewers: true,
                    MaxNumberOfFinancePlanReviewers: 10),
                NotificationPreference: new UpdatePreferenceRequest_NotificationPreference(
                    SendFinanceReportEnabled: true, SendFinanceReportInterval: 1),
                GeneralPreference: new UpdatePreferenceRequest_GeneralPreference(UseDarkMode: true)));

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