using Expenso.Api.Tests.E2E.TestData.Preferences;
using Expenso.UserPreferences.Core.Application.Preferences.Write.Commands.UpdatePreference.DTO.Request;

namespace Expenso.Api.Tests.E2E.UserPreferences.Preferences;

internal sealed class UpdatePreferences : PreferencesTestBase
{
    [Test]
    public async Task Should_ReturnExpectedResult()
    {
        // Arrange
        Guid? preferenceId = PreferencesDataProvider.PreferenceIds[1];
        _httpClient.SetFakeBearerToken(_claims);
        string requestPath = $"user-preferences/preferences/{preferenceId}";

        // Act
        HttpResponseMessage testResult = await _httpClient.PutAsJsonAsync(requestPath,
            new UpdatePreferenceRequest(new UpdatePreferenceRequest_FinancePreference(true, 5, true, 10),
                new UpdatePreferenceRequest_NotificationPreference(true, 1),
                new UpdatePreferenceRequest_GeneralPreference(true)));

        // Assert
        testResult.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Test]
    public async Task Should_Return401_When_NoAccessTokenProvided()
    {
        // Arrange
        Guid? preferenceId = PreferencesDataProvider.PreferenceIds[1];

        // Act
        HttpResponseMessage testResult =
            await _httpClient.PutAsync($"user-preferences/preferences/{preferenceId}", null);

        // Assert
        testResult.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}