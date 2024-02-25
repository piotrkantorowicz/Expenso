using Expenso.UserPreferences.Core.Application.Preferences.Write.Commands.CreatePreference.Internal.DTO.Request;
using Expenso.UserPreferences.Proxy.DTO.API.CreatePreference.Response;

namespace Expenso.Api.Tests.E2E.UserPreferences.Preferences;

internal sealed class CreatePreferences : PreferencesTestBase
{
    [Test]
    public async Task Should_ReturnExpectedResult()
    {
        // Arrange
        _httpClient.SetFakeBearerToken(_claims);
        Guid userId = Guid.NewGuid();
        const string requestPath = "user-preferences/preferences";

        // Act
        HttpResponseMessage testResult =
            await _httpClient.PostAsJsonAsync(requestPath, new CreatePreferenceRequest(userId));

        // Assert
        testResult.StatusCode.Should().Be(HttpStatusCode.Created);

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
        HttpResponseMessage testResult = await _httpClient.PostAsync(requestPath, null);

        // Assert
        testResult.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}