using Expenso.Api.Tests.E2E.TestData.Preferences;
using Expenso.UserPreferences.Proxy.DTO.API.GetPreference.Response;

namespace Expenso.Api.Tests.E2E.UserPreferences.Preferences;

internal sealed class GetPreferences : PreferencesTestBase
{
    [Test]
    public async Task Should_ReturnExpectedResult()
    {
        // Arrange
        Guid preferenceId = PreferencesDataInitializer.PreferenceIds[index: 3];
        _httpClient.SetFakeBearerToken(token: _claims);
        string requestPath = $"user-preferences/preferences/{preferenceId}";

        // Act
        HttpResponseMessage testResult = await _httpClient.GetAsync(requestUri: requestPath);

        // Assert
        testResult.StatusCode.Should().Be(expected: HttpStatusCode.OK);
        GetPreferenceResponse? testResultContent = await testResult.Content.ReadFromJsonAsync<GetPreferenceResponse>();
        testResultContent?.Id.Should().Be(expected: preferenceId);
    }

    [Test]
    public async Task Should_Return401_When_NoAccessTokenProvided()
    {
        // Arrange
        Guid preferenceId = PreferencesDataInitializer.PreferenceIds[index: 3];
        string requestPath = $"user-preferences/preferences/{preferenceId}";

        // Act
        HttpResponseMessage testResult = await _httpClient.GetAsync(requestUri: requestPath);

        // Assert
        testResult.StatusCode.Should().Be(expected: HttpStatusCode.Unauthorized);
    }
}