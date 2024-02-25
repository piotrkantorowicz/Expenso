using Expenso.Api.Tests.E2E.TestData;
using Expenso.UserPreferences.Proxy.DTO.API.GetPreference.Response;

namespace Expenso.Api.Tests.E2E.UserPreferences.Preferences;

internal sealed class GetUserPreferences : PreferencesTestBase
{
    [Test]
    public async Task Should_ReturnExpectedResult()
    {
        // Arrange
        Guid userId = UsersDataProvider.UserIds[2];
        _httpClient.SetFakeBearerToken(_claims);
        string request = $"user-preferences/preferences?userId={userId}";

        // Act
        HttpResponseMessage testResult = await _httpClient.GetAsync(request);

        // Assert
        testResult.StatusCode.Should().Be(HttpStatusCode.OK);
        GetPreferenceResponse? testResultContent = await testResult.Content.ReadFromJsonAsync<GetPreferenceResponse>();
        testResultContent?.UserId.Should().Be(userId);
    }

    [Test]
    public async Task Should_Return401_When_NoAccessTokenProvided()
    {
        // Arrange
        string request = $"user-preferences/preferences?userId={UsersDataProvider.UserIds[0]}";

        // Act
        HttpResponseMessage testResult = await _httpClient.GetAsync(request);

        // Assert
        testResult.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}