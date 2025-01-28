using System.Net;
using System.Net.Http.Json;

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
        _httpClient.SetFakeBearerToken(token: _claims);
        Guid userId = Guid.CreateVersion7();
        Guid preferenceId = Guid.CreateVersion7();

        // Act
        HttpResponseMessage response = await _httpClient.PostAsJsonAsync(requestUri: ApiRequestUrl,
            value: new CreatePreferenceRequest(PreferenceId: preferenceId, UserId: userId));

        // Assert
        AssertResponseCreated(response: response);

        CreatePreferenceResponse? responseContent =
            await response.Content.ReadFromJsonAsync<CreatePreferenceResponse>();

        responseContent.ShouldNotBeNull();
        responseContent?.PreferenceId.ShouldBe(expected: preferenceId);
    }

    [Test]
    public async Task Should_Return401_When_NoAccessTokenProvided()
    {
        // Arrange
        // Act
        HttpResponseMessage response = await _httpClient.PostAsync(requestUri: ApiRequestUrl, content: null);

        // Assert
        AssertResponseUnauthroised(response: response);
    }
}