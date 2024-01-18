using System.Text;

using Expenso.Api.Tests.E2E.TestData;
using Expenso.UserPreferences.Core.Application.DTO.UpdateUserPreferences;
using Expenso.UserPreferences.Proxy.Contracts.GetUserPreferences;

namespace Expenso.Api.Tests.E2E.UserPreferences.Preferences.Cases;

internal sealed class UpdatePreferences : PreferencesTestBase
{
    [Test]
    public async Task Should_ReturnExpectedResult()
    {
        // Arrange
        PreferenceContract? preference = PreferencesDataProvider.Preferences?[1];
        _httpClient.SetFakeBearerToken(_claims);

        // Act
        HttpResponseMessage testResult = await _httpClient.PutAsJsonAsync(
            new StringBuilder().Append("user-preferences/preferences/").Append(preference?.PreferenceId).ToString(),
            new UpdatePreferenceDto(new UpdateFinancePreferenceDto(true, 5, true, 10),
                new UpdateNotificationPreferenceDto(true, 1), new UpdateGeneralPreferenceDto(true)));

        // Assert
        testResult.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Test]
    public async Task Should_Return401_When_NoAccessTokenProvided()
    {
        // Arrange
        PreferenceContract? preferenceId = PreferencesDataProvider.Preferences?[1];

        // Act
        HttpResponseMessage testResult = await _httpClient.PutAsync(
            new StringBuilder().Append("user-preferences/preferences/").Append(preferenceId).ToString(), null);

        // Assert
        testResult.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}