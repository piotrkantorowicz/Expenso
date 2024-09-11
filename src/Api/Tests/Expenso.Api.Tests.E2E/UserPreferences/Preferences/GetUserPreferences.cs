using Expenso.Api.Configuration.Execution.Middlewares;
using Expenso.Api.Tests.E2E.TestData.IAM;
using Expenso.UserPreferences.Proxy.DTO.API.GetPreference.Response;

namespace Expenso.Api.Tests.E2E.UserPreferences.Preferences;

internal sealed class GetUserPreferences : PreferencesTestBase
{
    [Test]
    public async Task Should_ReturnExpectedResult()
    {
        // Arrange
        Guid userId = UserDataInitializer.UserIds[index: 2];
        _httpClient.SetFakeBearerToken(token: _claims);
        string requestPath = $"user-preferences/preferences?userId={userId}";

        // Act
        HttpResponseMessage response = await _httpClient.GetAsync(requestUri: requestPath);

        // Assert
        response.StatusCode.Should().Be(expected: HttpStatusCode.OK);
        GetPreferenceResponse? responseContent = await response.Content.ReadFromJsonAsync<GetPreferenceResponse>();
        response.Headers.Contains(name: CorrelationIdMiddleware.CorrelationHeaderKey).Should().BeTrue();
        responseContent?.UserId.Should().Be(expected: userId);
    }

    [Test]
    public async Task Should_Return401_When_NoAccessTokenProvided()
    {
        // Arrange
        string requestPath = $"user-preferences/preferences?userId={UserDataInitializer.UserIds[index: 0]}";

        // Act
        HttpResponseMessage response = await _httpClient.GetAsync(requestUri: requestPath);

        // Assert
        AssertResponseUnauthroised(response: response);
    }
}