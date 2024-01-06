using System.Text;

using Expenso.Api.Tests.E2E.TestData;
using Expenso.UserPreferences.Core.DTO.GetUserPreferences;
using Expenso.UserPreferences.Proxy.Contracts.GetUserPreferences;

namespace Expenso.Api.Tests.E2E.UserPreferences.Preferences.Cases;

internal sealed class GetPreferences : PreferencesTestBase
{
    [Test]
    public async Task Should_ReturnExpectedResult()
    {
        // Arrange
        PreferenceContract? preference = PreferencesDataProvider.Preferences?[4];
        _httpClient.SetFakeBearerToken(_claims);

        string request = new StringBuilder()
            .Append("user-preferences/preferences/")
            .Append(preference?.PreferencesId)
            .ToString();

        // Act
        HttpResponseMessage testResult = await _httpClient.GetAsync(request);

        // Assert
        testResult.StatusCode.Should().Be(HttpStatusCode.OK);
        PreferenceDto? testResultContent = await testResult.Content.ReadFromJsonAsync<PreferenceDto>();
        testResultContent?.Should().BeEquivalentTo(preference);
    }

    [Test]
    public async Task Should_Return401_When_NoAccessTokenProvided()
    {
        // Arrange
        PreferenceContract? preference = PreferencesDataProvider.Preferences?[4];

        string request = new StringBuilder()
            .Append("user-preferences/preferences/")
            .Append(preference?.PreferencesId)
            .ToString();

        // Act
        HttpResponseMessage testResult = await _httpClient.GetAsync(request);

        // Assert
        testResult.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}