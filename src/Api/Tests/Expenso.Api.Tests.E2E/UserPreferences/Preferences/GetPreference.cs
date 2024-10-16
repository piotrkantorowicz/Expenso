using Expenso.Api.Tests.E2E.TestData.Preferences;
using Expenso.UserPreferences.Core.Application.Preferences.Read.Queries.GetPreference.DTO.Response;

namespace Expenso.Api.Tests.E2E.UserPreferences.Preferences;

internal sealed class GetPreference : PreferencesTestBase
{
    [Test]
    public async Task Should_ReturnExpectedResult_When_PassPreferenceId()
    {
        // Arrange
        Guid preferenceId = PreferencesDataInitializer.PreferenceIds[index: 3];
        _httpClient.SetFakeBearerToken(token: _claims);
        string requestPath = $"user-preferences/preferences/{preferenceId}";

        // Act
        HttpResponseMessage response = await _httpClient.GetAsync(requestUri: requestPath);

        // Assert
        AssertResponseOk(response: response);
        GetPreferenceResponse? responseContent = await response.Content.ReadFromJsonAsync<GetPreferenceResponse>();
        responseContent?.Id.Should().Be(expected: preferenceId);
    }

    [Test]
    public async Task Should_Return401_When_NoAccessTokenProvided()
    {
        // Arrange
        Guid preferenceId = PreferencesDataInitializer.PreferenceIds[index: 3];
        string requestPath = $"user-preferences/preferences/{preferenceId}";

        // Act
        HttpResponseMessage response = await _httpClient.GetAsync(requestUri: requestPath);

        // Assert
        AssertResponseUnauthroised(response: response);
    }
}