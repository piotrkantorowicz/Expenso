using Expenso.UserPreferences.Proxy.DTO.API.CreatePreference.Request;
using Expenso.UserPreferences.Proxy.DTO.API.CreatePreference.Response;

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
        HttpResponseMessage testResult = await _httpClient.PostAsJsonAsync(requestUri: requestPath,
            value: new CreatePreferenceRequest(UserId: userId));

        // Assert
        testResult.StatusCode.Should().Be(expected: HttpStatusCode.Created);

        CreatePreferenceResponse? testResultContent =
            await testResult.Content.ReadFromJsonAsync<CreatePreferenceResponse>();

        testResultContent.Should().NotBeNull();
    }

    [Test]
    public async Task Should_Return401_When_NoAccessTokenProvided()
    {
        // Arrange
        const string requestPath = "user-preferences/preferences";

        // Act
        HttpResponseMessage testResult = await _httpClient.PostAsync(requestUri: requestPath, content: null);

        // Assert
        testResult.StatusCode.Should().Be(expected: HttpStatusCode.Unauthorized);
    }
}