using Expenso.Api.Configuration.Execution.Middlewares;
using Expenso.Api.Tests.E2E.TestData.Preferences;
using Expenso.UserPreferences.Proxy.DTO.API.GetPreference.Response;

namespace Expenso.Api.Tests.E2E.UserPreferences.Preferences;

internal sealed class GetCurrentUserPreferences : PreferencesTestBase
{
    [Test]
    public async Task Should_ReturnExpectedResult()
    {
        // Arrange
        Guid preferenceId = PreferencesDataInitializer.PreferenceIds[index: 3];
        _httpClient.SetFakeBearerToken(token: _claims);

        // Act
        HttpResponseMessage response =
            await _httpClient.GetAsync(requestUri: "user-preferences/preferences/current-user");

        // Assert
        response.StatusCode.Should().Be(expected: HttpStatusCode.OK);
        GetPreferenceResponse? responseContent = await response.Content.ReadFromJsonAsync<GetPreferenceResponse>();
        response.Headers.Contains(name: CorrelationIdMiddleware.CorrelationHeaderKey).Should().BeTrue();
        responseContent?.Id.Should().Be(expected: preferenceId);
    }

    [Test]
    public async Task Should_Return401_When_NoAccessTokenProvided()
    {
        // Arrange
        // Act
        HttpResponseMessage response =
            await _httpClient.GetAsync(requestUri: "user-preferences/preferences/current-user");

        // Assert
        AssertResponseUnauthroised(response: response);
    }
}