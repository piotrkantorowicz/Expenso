using System.Net;
using System.Net.Http.Json;

using Expenso.Api.Tests.E2E.TestData.IAM;
using Expenso.Api.Tests.E2E.TestData.Preferences;
using Expenso.UserPreferences.Shared.DTO.API.CreatePreference.Request;
using Expenso.UserPreferences.Shared.DTO.API.CreatePreference.Response;

using NUnit.Framework;

using Shouldly;

namespace Expenso.Api.Tests.E2E.UserPreferences.Preferences;

[TestFixture]
internal sealed class CreatePreferences : PreferencesTestBase
{
    [Test]
    public async Task Should_ReturnExpectedResult()
    {
        // Arrange
        _httpClient.SetFakeBearerToken(token: Claims);
        Guid userId = Guid.CreateVersion7();
        Guid preferenceId = Guid.CreateVersion7();

        // Act
        HttpResponseMessage response = await _httpClient.PostAsJsonAsync(requestUri: ApiRequestUrl,
            value: new CreatePreferenceRequest(PreferenceId: preferenceId, UserId: userId));

        // Assert
        AssertResponse(response: response, statusCode: HttpStatusCode.Created);

        CreatePreferenceResponse? responseContent =
            await response.Content.ReadFromJsonAsync<CreatePreferenceResponse>();

        responseContent?.ShouldNotBeNull();
        responseContent?.PreferenceId.ShouldBe(expected: preferenceId);
    }

    [Test]
    public async Task Should_Return409_When_ResourceWithProvidedPreferenceIdAlreadyExists()
    {
        // Arrange
        _httpClient.SetFakeBearerToken(token: Claims);
        Guid userId = Guid.CreateVersion7();

        // Act
        HttpResponseMessage response = await _httpClient.PostAsJsonAsync(requestUri: ApiRequestUrl,
            value: new CreatePreferenceRequest(PreferenceId: PreferencesDataInitializer.PreferenceIds[index: 0],
                UserId: userId));

        // Assert
        AssertResponse(response: response, statusCode: HttpStatusCode.Conflict);
    }

    [Test]
    public async Task Should_Return409_When_ResourceWithProvidedUserIdAlreadyExists()
    {
        // Arrange
        _httpClient.SetFakeBearerToken(token: Claims);
        Guid preferenceId = Guid.CreateVersion7();

        // Act
        HttpResponseMessage response = await _httpClient.PostAsJsonAsync(requestUri: ApiRequestUrl,
            value: new CreatePreferenceRequest(PreferenceId: preferenceId,
                UserId: UserDataInitializer.UserIds[index: 0]));

        // Assert
        AssertResponse(response: response, statusCode: HttpStatusCode.Conflict);
    }
    [Test]
    public async Task Should_Return401_When_NoAccessTokenProvided()
    {
        // Arrange
        // Act
        HttpResponseMessage response = await _httpClient.PostAsync(requestUri: ApiRequestUrl, content: null);

        // Assert
        AssertResponseStatusCode(response: response, statusCode: HttpStatusCode.Unauthorized);
    }
}