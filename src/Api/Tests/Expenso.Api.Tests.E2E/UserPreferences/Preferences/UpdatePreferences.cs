using System.Net;
using System.Net.Http.Json;

using Expenso.Api.Tests.E2E.TestData.Preferences;
using Expenso.UserPreferences.Shared.DTO.API.UpdatePreference;

using NUnit.Framework;

namespace Expenso.Api.Tests.E2E.UserPreferences.Preferences;

[TestFixture]
internal sealed class UpdatePreferences : PreferencesTestBase
{
    [Test]
    public async Task Should_ReturnExpectedResult()
    {
        // Arrange
        Guid? preferenceId = PreferencesDataInitializer.PreferenceIds[index: 1];
        _httpClient.SetFakeBearerToken(token: Claims);

        // Act
        HttpResponseMessage response = await _httpClient.PutAsJsonAsync(requestUri: $"{ApiRequestUrl}/{preferenceId}",
            value: new UpdatePreferenceRequest(FinancePreference: new UpdatePreferenceRequestFinancePreference(
                    AllowAddFinancePlanSubOwners: true,
                    MaxNumberOfSubFinancePlanSubOwners: 5, AllowAddFinancePlanReviewers: true,
                    MaxNumberOfFinancePlanReviewers: 10),
                NotificationPreference: new UpdatePreferenceRequestNotificationPreference(
                    SendFinanceReportEnabled: true, SendFinanceReportInterval: 1),
                GeneralPreference: new UpdatePreferenceRequestGeneralPreference(UseDarkMode: true)));

        // Assert
        AssertResponse(response: response, statusCode: HttpStatusCode.NoContent);
    }

    [Test]
    public async Task Should_Return401_When_NoAccessTokenProvided()
    {
        // Arrange
        Guid? preferenceId = PreferencesDataInitializer.PreferenceIds[index: 1];

        // Act
        HttpResponseMessage response =
            await _httpClient.PutAsync(requestUri: $"{ApiRequestUrl}/{preferenceId}", content: null);

        // Assert
        AssertResponseStatusCode(response: response, statusCode: HttpStatusCode.Unauthorized);
    }
}