using System.Net;
using System.Net.Http.Json;

using Expenso.Api.Tests.E2E.TestData.IAM;
using Expenso.Api.Tests.E2E.TestData.Preferences;
using Expenso.UserPreferences.Core.Application.Preferences.Read.Queries.GetPreference.DTO.Response;

using NUnit.Framework;

using Shouldly;

namespace Expenso.Api.Tests.E2E.UserPreferences.Preferences;

internal sealed class GetPreferences : PreferencesTestBase
{
    [Test]
    public async Task Should_Return200_When_PreferenceIdProvided_And_InputIsValid()
    {
        // Arrange
        Guid preferenceId = PreferencesDataInitializer.PreferenceIds[index: 3];
        _httpClient.SetFakeBearerToken(token: _claimsService.GetClaims());

        // Act
        HttpResponseMessage response = await _httpClient.GetAsync(requestUri: $"{ApiRequestUrl}?id={preferenceId}");

        // Assert
        AssertResponse(response: response, statusCode: HttpStatusCode.OK);
        GetPreferenceResponse? responseContent = await response.Content.ReadFromJsonAsync<GetPreferenceResponse>();
        responseContent?.Id.ShouldBe(expected: preferenceId);
    }

    [Test]
    public async Task Should_Return200_When_UserIdProvided_And_InputIsValid()
    {
        // Arrange
        Guid userId = UserDataInitializer.UserIds[index: 2];
        _httpClient.SetFakeBearerToken(token: _claimsService.GetClaims());

        // Act
        HttpResponseMessage response = await _httpClient.GetAsync(requestUri: $"{ApiRequestUrl}?userId={userId}");

        // Assert
        AssertResponse(response: response, statusCode: HttpStatusCode.OK);
        GetPreferenceResponse? responseContent = await response.Content.ReadFromJsonAsync<GetPreferenceResponse>();
        responseContent?.UserId.ShouldBe(expected: userId);
    }

    [Test]
    public async Task Should_Return404_When_PreferenceIdProvided_ResourceNotFound()
    {
        // Arrange
        Guid preferenceId = Guid.CreateVersion7();
        _httpClient.SetFakeBearerToken(token: _claimsService.GetClaims());

        // Act
        HttpResponseMessage response = await _httpClient.GetAsync(requestUri: $"{ApiRequestUrl}?id={preferenceId}");

        // Assert
        AssertResponse(response: response, statusCode: HttpStatusCode.NotFound);
    }

    [Test]
    public async Task Should_Return404_When_UserIdProvided_ResourceNotFound()
    {
        // Arrange
        Guid userId = Guid.CreateVersion7();
        _httpClient.SetFakeBearerToken(token: _claimsService.GetClaims());

        // Act
        HttpResponseMessage response = await _httpClient.GetAsync(requestUri: $"{ApiRequestUrl}?userId={userId}");

        // Assert
        AssertResponse(response: response, statusCode: HttpStatusCode.NotFound);
    }

    [Test]
    public async Task Should_Return401_When_NoAccessTokenProvided()
    {
        // Arrange
        Guid preferenceId = PreferencesDataInitializer.PreferenceIds[index: 3];

        // Act
        HttpResponseMessage response = await _httpClient.GetAsync(requestUri: $"{ApiRequestUrl}?id={preferenceId}");

        // Assert
        AssertResponseStatusCode(response: response, statusCode: HttpStatusCode.Unauthorized);
    }
}