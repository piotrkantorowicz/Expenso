using Expenso.Api.Tests.E2E.TestData;
using Expenso.UserPreferences.Core.Application.Preferences.DTO.GetPreferences.Response;

namespace Expenso.Api.Tests.E2E.UserPreferences.Preferences.Cases;

internal sealed class GetCurrentUserPreferences : PreferencesTestBase
{
    [Test]
    public async Task Should_ReturnExpectedResult()
    {
        // Arrange
        Guid preferenceId = PreferencesDataProvider.PreferenceIds[3];
        _httpClient.SetFakeBearerToken(_claims);

        // Act
        HttpResponseMessage testResult = await _httpClient.GetAsync("user-preferences/preferences/current-user");

        // Assert
        testResult.StatusCode.Should().Be(HttpStatusCode.OK);
        GetPreferenceResponse? testResultContent = await testResult.Content.ReadFromJsonAsync<GetPreferenceResponse>();
        testResultContent?.Id.Should().Be(preferenceId);
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