using System.Net;
using System.Net.Http.Json;

using Expenso.Api.Configuration.Auth.Claims;
using Expenso.Api.Tests.E2E.TestData.Preferences;
using Expenso.UserPreferences.Core.Application.Preferences.Read.Queries.GetPreference.DTO.Response;

using NUnit.Framework;

using Shouldly;

namespace Expenso.Api.Tests.E2E.UserPreferences.Preferences;

[TestFixture]
internal sealed class GetCurrentUserPreferences : PreferencesTestBase
{
    [Test]
    public async Task Should_Return200_When_InputIsValid()
    {
        // Arrange
        _httpClient.SetFakeBearerToken(token: _claimsService.GetClaims());

        // Act
        HttpResponseMessage response = await _httpClient.GetAsync(requestUri: $"{ApiRequestUrl}/current-user");

        // Assert
        AssertResponse(response: response, statusCode: HttpStatusCode.OK);
        GetPreferenceResponse? responseContent = await response.Content.ReadFromJsonAsync<GetPreferenceResponse>();
        responseContent?.Id.ShouldBe(expected: PreferencesDataInitializer.PreferenceIds[index: 3]);
    }

    [Test]
    public async Task Should_Return404_When_UserIdMissing()
    {
        // Arrange
        _claimsService.Remove(key: ClaimNames.UserIdClaimName);
        _httpClient.SetFakeBearerToken(token: _claimsService.GetClaims());

        // Act
        HttpResponseMessage response = await _httpClient.GetAsync(requestUri: $"{ApiRequestUrl}/current-user");

        // Assert
        AssertResponse(response: response, statusCode: HttpStatusCode.NotFound);
    }

    [Test]
    public async Task Should_Return404_When_PreferenceMissing()
    {
        // Arrange
        _claimsService.Replace(key: ClaimNames.UserIdClaimName, value: Guid.CreateVersion7().ToString());
        _httpClient.SetFakeBearerToken(token: _claimsService.GetClaims());

        // Act
        HttpResponseMessage response = await _httpClient.GetAsync(requestUri: $"{ApiRequestUrl}/current-user");

        // Assert
        AssertResponse(response: response, statusCode: HttpStatusCode.NotFound);
    }

    [Test]
    public async Task Should_Return401_When_NoAccessTokenProvided()
    {
        // Arrange
        // Act
        HttpResponseMessage response = await _httpClient.GetAsync(requestUri: $"{ApiRequestUrl}/current-user");

        // Assert
        AssertResponseStatusCode(response: response, statusCode: HttpStatusCode.Unauthorized);
    }
}