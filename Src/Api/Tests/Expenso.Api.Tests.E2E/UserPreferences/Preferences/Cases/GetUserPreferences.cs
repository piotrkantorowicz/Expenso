using System.Text;

using Expenso.Api.Tests.E2E.TestData;
using Expenso.UserPreferences.Core.Application.Preferences.DTO.GetPreferences.Response;

namespace Expenso.Api.Tests.E2E.UserPreferences.Preferences.Cases;

internal sealed class GetUserPreferences : PreferencesTestBase
{
    [Test]
    public async Task Should_ReturnExpectedResult()
    {
        // Arrange
        Guid userId = UsersDataProvider.UserIds[2];
        _httpClient.SetFakeBearerToken(_claims);
        string request = new StringBuilder().Append("user-preferences/preferences?userId=").Append(userId).ToString();
        
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
        string request = new StringBuilder()
            .Append("user-preferences/preferences?userId=")
            .Append(UsersDataProvider.UserIds[0])
            .ToString();

        // Act
        HttpResponseMessage testResult = await _httpClient.GetAsync(request);

        // Assert
        testResult.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}