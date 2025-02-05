using System.Net;
using System.Net.Http.Json;

using Expenso.Api.Tests.E2E.TestData.Preferences;
using Expenso.UserPreferences.Core.Application.Preferences.Read.Queries.GetPreference.DTO.Response;

using NUnit.Framework;

using Shouldly;

namespace Expenso.Api.Tests.E2E.UserPreferences.Preferences;

internal sealed class GetPreference : PreferencesTestBase
{
    [Test]
    public async Task Should_Return200_When_InputIsValid_When_PreferenceIdHasBeenProvided()
    {
        // Arrange
        Guid preferenceId = PreferencesDataInitializer.PreferenceIds[index: 3];
        _httpClient.SetFakeBearerToken(token: _claimsService.GetClaims());

        // Act
        HttpResponseMessage response = await _httpClient.GetAsync(requestUri: $"{ApiRequestUrl}/{preferenceId}");

        // Assert
        AssertResponse(response: response, statusCode: HttpStatusCode.OK);
        GetPreferenceResponse? responseContent = await response.Content.ReadFromJsonAsync<GetPreferenceResponse>();
        responseContent?.Id.ShouldBe(expected: preferenceId);
    }

    [Test]
    public async Task Should_Return404_When_PreferenceHasNotBeenFound()
    {
        // Arrange
        Guid preferenceId = Guid.CreateVersion7();
        _httpClient.SetFakeBearerToken(token: _claimsService.GetClaims());

        // Act
        HttpResponseMessage response = await _httpClient.GetAsync(requestUri: $"{ApiRequestUrl}/{preferenceId}");

        // Assert
        AssertResponse(response: response, statusCode: HttpStatusCode.NotFound);
    }

    [Test]
    public async Task Should_Return401_When_NoAccessTokenProvided()
    {
        // Arrange
        // Act
        HttpResponseMessage response =
            await _httpClient.GetAsync(
                requestUri: $"{ApiRequestUrl}/{PreferencesDataInitializer.PreferenceIds[index: 3]}");

        // Assert
        AssertResponseStatusCode(response: response, statusCode: HttpStatusCode.Unauthorized);
    }
}