using System.Text;

using Expenso.Api.Tests.E2E.TestData;
using Expenso.UserPreferences.Core.Application.Preferences.DTO.UpdatePreferences.Request;

namespace Expenso.Api.Tests.E2E.UserPreferences.Preferences.Cases;

internal sealed class UpdatePreferences : PreferencesTestBase
{
    [Test]
    public async Task Should_ReturnExpectedResult()
    {
        // Arrange
        Guid? preferenceId = PreferencesDataProvider.PreferenceIds[1];
        _httpClient.SetFakeBearerToken(_claims);
        string request = new StringBuilder().Append("user-preferences/preferences/").Append(preferenceId).ToString();

        // Act
        HttpResponseMessage testResult = await _httpClient.PutAsJsonAsync(request,
            new UpdatePreferenceRequest(new UpdateFinancePreferenceRequest(true, 5, true, 10),
                new UpdateNotificationPreferenceRequest(true, 1), new UpdateGeneralPreferenceRequest(true)));

        // Assert
        testResult.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Test]
    public async Task Should_Return401_When_NoAccessTokenProvided()
    {
        // Arrange
        Guid? preferenceId = PreferencesDataProvider.PreferenceIds[1];

        // Act
        HttpResponseMessage testResult = await _httpClient.PutAsync(
            new StringBuilder().Append("user-preferences/preferences/").Append(preferenceId).ToString(), null);

        // Assert
        testResult.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}