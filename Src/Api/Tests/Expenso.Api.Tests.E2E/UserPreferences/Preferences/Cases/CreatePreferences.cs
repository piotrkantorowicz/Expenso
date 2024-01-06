using System.Text;

using Expenso.UserPreferences.Core.DTO.GetUserPreferences;

namespace Expenso.Api.Tests.E2E.UserPreferences.Preferences.Cases;

internal sealed class CreatePreferences : PreferencesTestBase
{
    [Test]
    public async Task Should_ReturnExpectedResult()
    {
        // Arrange
        _httpClient.SetFakeBearerToken(_claims);
        Guid userId = Guid.NewGuid();
        string request = new StringBuilder().Append("user-preferences/preferences/").Append(userId).ToString();
        
        // Act
        HttpResponseMessage testResult = await _httpClient.PostAsync(request, null);

        // Assert
        testResult.StatusCode.Should().Be(HttpStatusCode.Created);
        PreferenceDto? testResultContent = await testResult.Content.ReadFromJsonAsync<PreferenceDto>();
        testResultContent?.UserId.Should().Be(userId);
        testResultContent?.FinancePreference.Should().BeEquivalentTo(new FinancePreferenceDto(false, 0, false, 0));
        testResultContent?.NotificationPreference.Should().BeEquivalentTo(new NotificationPreferenceDto(true, 7));
        testResultContent?.GeneralPreference.Should().BeEquivalentTo(new GeneralPreferenceDto(false));
    }

    [Test]
    public async Task Should_Return401_When_NoAccessTokenProvided()
    {
        // Arrange
        Guid preferenceId = Guid.NewGuid();

        // Act
        HttpResponseMessage testResult = await _httpClient.PostAsync(
            new StringBuilder().Append("user-preferences/preferences/").Append(preferenceId).ToString(), null);

        // Assert
        testResult.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}