using Expenso.UserPreferences.Proxy.DTO.API.CreatePreference.Requests;
using Expenso.UserPreferences.Proxy.DTO.API.CreatePreference.Responses;

namespace Expenso.Api.Tests.E2E.UserPreferences.Preferences;

internal sealed class CreatePreferences : PreferencesTestBase
{
    [Test]
    public async Task Should_ReturnExpectedResult()
    {
        // Arrange
        _httpClient.SetFakeBearerToken(token: _claims);
        Guid userId = Guid.NewGuid();
        const string requestPath = "user-preferences/preferences";

        // Act
        HttpResponseMessage response = await _httpClient.PostAsJsonAsync(requestUri: requestPath,
            value: new CreatePreferenceRequest(UserId: userId));

        // Assert
        AssertResponseCreated(response: response);

        CreatePreferenceResponse? responseContent =
            await response.Content.ReadFromJsonAsync<CreatePreferenceResponse>();

        responseContent.Should().NotBeNull();
    }

    [Test]
    public async Task Should_Return401_When_NoAccessTokenProvided()
    {
        // Arrange
        const string requestPath = "user-preferences/preferences";

        // Act
        HttpResponseMessage response = await _httpClient.PostAsync(requestUri: requestPath, content: null);

        // Assert
        AssertResponseUnauthroised(response: response);
    }
}