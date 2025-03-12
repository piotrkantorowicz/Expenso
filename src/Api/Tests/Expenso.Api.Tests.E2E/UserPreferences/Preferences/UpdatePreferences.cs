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
    public async Task Should_Return204_When_InputIsValid()
    {
        // Arrange
        Guid? preferenceId = PreferencesDataInitializer.PreferenceIds[index: 1];
        _httpClient.SetFakeBearerToken(token: _claimsService.GetClaims());

        // Act
        HttpResponseMessage response = await _httpClient.PutAsJsonAsync(requestUri: $"{ApiRequestUrl}/{preferenceId}",
            value: CreateUpdatePreferenceRequest());

        // Assert
        AssertResponse(response: response, statusCode: HttpStatusCode.NoContent);
    }

    [Test]
    public async Task Should_Return400_When_RequestIsNull()
    {
        // Arrange
        Guid? preferenceId = PreferencesDataInitializer.PreferenceIds[index: 1];
        _httpClient.SetFakeBearerToken(token: _claimsService.GetClaims());

        // Act
        HttpResponseMessage response =
            await _httpClient.PutAsync(requestUri: $"{ApiRequestUrl}/{preferenceId}", content: null);

        // Assert
        AssertResponse(response: response, statusCode: HttpStatusCode.BadRequest);
    }

    [Test]
    public async Task Should_Return404_When_ResourceHasNotBeenFound()
    {
        // Arrange
        Guid? preferenceId = Guid.CreateVersion7();
        _httpClient.SetFakeBearerToken(token: _claimsService.GetClaims());

        // Act
        HttpResponseMessage response = await _httpClient.PutAsJsonAsync(requestUri: $"{ApiRequestUrl}/{preferenceId}",
            value: CreateUpdatePreferenceRequest());

        // Assert
        AssertResponse(response: response, statusCode: HttpStatusCode.NotFound);
    }

    [Test]
    public async Task Should_Return422_When_RequestPathHasBeenEmpty()
    {
        // Arrange
        Guid? preferenceId = Guid.Empty;
        _httpClient.SetFakeBearerToken(token: _claimsService.GetClaims());

        // Act
        HttpResponseMessage response = await _httpClient.PutAsJsonAsync(requestUri: $"{ApiRequestUrl}/{preferenceId}",
            value: CreateUpdatePreferenceRequest());

        // Assert
        AssertResponse(response: response, statusCode: HttpStatusCode.UnprocessableEntity);
    }

    [Test, TestCaseSource(sourceName: nameof(InvalidUpdatePreferenceRequestCases))]
    public async Task Should_HandleInvalidRequest(UpdatePreferenceRequest request)
    {
        // Arrange
        Guid? preferenceId = PreferencesDataInitializer.PreferenceIds[index: 1];
        _httpClient.SetFakeBearerToken(token: _claimsService.GetClaims());

        // Act
        HttpResponseMessage response =
            await _httpClient.PutAsJsonAsync(requestUri: $"{ApiRequestUrl}/{preferenceId}", value: request);

        // Assert
        AssertResponse(response: response, statusCode: HttpStatusCode.UnprocessableEntity);
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

    private static UpdatePreferenceRequest CreateUpdatePreferenceRequest()
    {
        return new UpdatePreferenceRequest(
            FinancePreference: new UpdatePreferenceRequestFinancePreference(AllowAddFinancePlanSubOwners: true,
                MaxNumberOfSubFinancePlanSubOwners: 5, AllowAddFinancePlanReviewers: true,
                MaxNumberOfFinancePlanReviewers: 10),
            NotificationPreference: new UpdatePreferenceRequestNotificationPreference(SendFinanceReportEnabled: true,
                SendFinanceReportInterval: 1),
            GeneralPreference: new UpdatePreferenceRequestGeneralPreference(UseDarkMode: true));
    }

    private static IEnumerable<object> InvalidUpdatePreferenceRequestCases()
    {
        yield return new TestCaseData(arg: new UpdatePreferenceRequest(
            FinancePreference: new UpdatePreferenceRequestFinancePreference(AllowAddFinancePlanSubOwners: true,
                MaxNumberOfSubFinancePlanSubOwners: 6, AllowAddFinancePlanReviewers: true,
                MaxNumberOfFinancePlanReviewers: 10),
            NotificationPreference: new UpdatePreferenceRequestNotificationPreference(SendFinanceReportEnabled: true,
                SendFinanceReportInterval: 32),
            GeneralPreference: new UpdatePreferenceRequestGeneralPreference(UseDarkMode: true))).SetName(
            name: "Should_Return422_When_FinancePreference_MaxNumberOfSubFinancePlanSubOwners_IsInvalid");

        yield return new TestCaseData(arg: new UpdatePreferenceRequest(
            FinancePreference: new UpdatePreferenceRequestFinancePreference(AllowAddFinancePlanSubOwners: true,
                MaxNumberOfSubFinancePlanSubOwners: 5, AllowAddFinancePlanReviewers: true,
                MaxNumberOfFinancePlanReviewers: 11),
            NotificationPreference: new UpdatePreferenceRequestNotificationPreference(SendFinanceReportEnabled: true,
                SendFinanceReportInterval: 1),
            GeneralPreference: new UpdatePreferenceRequestGeneralPreference(UseDarkMode: true))).SetName(
            name: "Should_Return422_When_FinancePreference_MaxNumberOfFinancePlanReviewers_IsInvalid");

        yield return new TestCaseData(arg: new UpdatePreferenceRequest(
            FinancePreference: new UpdatePreferenceRequestFinancePreference(AllowAddFinancePlanSubOwners: true,
                MaxNumberOfSubFinancePlanSubOwners: 5, AllowAddFinancePlanReviewers: true,
                MaxNumberOfFinancePlanReviewers: 10),
            NotificationPreference: new UpdatePreferenceRequestNotificationPreference(SendFinanceReportEnabled: true,
                SendFinanceReportInterval: 32),
            GeneralPreference: new UpdatePreferenceRequestGeneralPreference(UseDarkMode: true))).SetName(
            name: "Should_Return422_When_NotificationPreference_SendFinanceReportInterval_IsInvalid");
    }
}