using Expenso.Api.Tests.E2E.TestData;
using Expenso.UserPreferences.Core.Application.DTO.GetUserPreferences;
using Expenso.UserPreferences.Proxy.Contracts.GetUserPreferences;

namespace Expenso.Api.Tests.E2E.UserPreferences.Preferences.Cases;

internal sealed class GetCurrentUserPreferences : PreferencesTestBase
{
    [Test]
    public async Task Should_ReturnExpectedResult()
    {
        // Arrange
        PreferenceContract? preference = PreferencesDataProvider.Preferences?[3];
        _httpClient.SetFakeBearerToken(_claims);

        // Act
        HttpResponseMessage testResult = await _httpClient.GetAsync("user-preferences/preferences/current-user");

        // Assert
        testResult.StatusCode.Should().Be(HttpStatusCode.OK);
        PreferenceDto? testResultContent = await testResult.Content.ReadFromJsonAsync<PreferenceDto>();
        testResultContent?.Should().BeEquivalentTo(preference);
    }

    [Test]
    public async Task Should_Return401_When_NoAccessTokenProvided()
    {
        // Arrange
        // Act
        HttpResponseMessage testResult = await _httpClient.GetAsync("user-preferences/preferences/current-user");

        // Assert
        testResult.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}