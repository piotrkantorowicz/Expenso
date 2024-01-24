using System.Text;

using Expenso.Api.Tests.E2E.TestData;
using Expenso.UserPreferences.Core.Application.Preferences.DTO.GetPreferences.Response;

namespace Expenso.Api.Tests.E2E.UserPreferences.Preferences;

internal sealed class GetPreferences : PreferencesTestBase
{
    [Test]
    public async Task Should_ReturnExpectedResult()
    {
        // Arrange
        Guid preferenceId = PreferencesDataProvider.PreferenceIds[3];
        _httpClient.SetFakeBearerToken(_claims);
        string request = new StringBuilder().Append("user-preferences/preferences/").Append(preferenceId).ToString();

        // Act
        HttpResponseMessage testResult = await _httpClient.GetAsync(request);

        // Assert
        testResult.StatusCode.Should().Be(HttpStatusCode.OK);
        GetPreferenceResponse? testResultContent = await testResult.Content.ReadFromJsonAsync<GetPreferenceResponse>();
        testResultContent?.Id.Should().Be(preferenceId);
    }

    [Test]
    public async Task Should_Return401_When_NoAccessTokenProvided()
    {
        // Arrange
        Guid preferenceId = PreferencesDataProvider.PreferenceIds[3];
        string request = new StringBuilder().Append("user-preferences/preferences/").Append(preferenceId).ToString();

        // Act
        HttpResponseMessage testResult = await _httpClient.GetAsync(request);

        // Assert
        testResult.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}