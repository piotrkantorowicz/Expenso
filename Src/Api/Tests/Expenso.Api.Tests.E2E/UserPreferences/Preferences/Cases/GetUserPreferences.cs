using System.Text;

using Expenso.Api.Tests.E2E.TestData;
using Expenso.UserPreferences.Core.DTO.GetUserPreferences;
using Expenso.UserPreferences.Proxy.Contracts.GetUserPreferences;

namespace Expenso.Api.Tests.E2E.UserPreferences.Preferences.Cases;

internal sealed class GetUserPreferences : PreferencesTestBase
{
    [Test]
    public async Task Should_ReturnExpectedResult()
    {
        // Arrange
        Guid userId = UsersDataProvider.UserIds[2];
        PreferenceContract? preference = PreferencesDataProvider.Preferences?[2];
        _httpClient.SetFakeBearerToken(_claims);

        // Act
        HttpResponseMessage testResult = await _httpClient.GetAsync(new StringBuilder()
            .Append("user-preferences/preferences?userId=")
            .Append(userId)
            .ToString());

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
        HttpResponseMessage testResult = await _httpClient.GetAsync(new StringBuilder()
            .Append("user-preferences/preferences?userId=")
            .Append(UsersDataProvider.UserIds[0])
            .ToString());

        // Assert
        testResult.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}