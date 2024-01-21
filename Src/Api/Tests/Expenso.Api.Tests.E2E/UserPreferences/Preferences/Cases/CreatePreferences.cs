using System.Text;

using Expenso.UserPreferences.Core.Application.Preferences.DTO.CreatePreference.Request;
using Expenso.UserPreferences.Core.Application.Preferences.DTO.CreatePreference.Response;
using Expenso.UserPreferences.Core.Application.Preferences.DTO.GetPreferences.Response;

namespace Expenso.Api.Tests.E2E.UserPreferences.Preferences.Cases;

internal sealed class CreatePreferences : PreferencesTestBase
{
    [Test]
    public async Task Should_ReturnExpectedResult()
    {
        // Arrange
        _httpClient.SetFakeBearerToken(_claims);
        Guid userId = Guid.NewGuid();
        string request = new StringBuilder().Append("user-preferences/preferences").ToString();

        // Act
        HttpResponseMessage testResult =
            await _httpClient.PostAsJsonAsync(request, new CreatePreferenceRequest(userId));

        // Assert
        testResult.StatusCode.Should().Be(HttpStatusCode.Created);

        CreatePreferenceResponse? testResultContent =
            await testResult.Content.ReadFromJsonAsync<CreatePreferenceResponse>();

        testResultContent?.UserId.Should().Be(userId);

        testResultContent
            ?.FinancePreference.Should()
            .BeEquivalentTo(new GetFinancePreferenceResponse(false, 0, false, 0));

        testResultContent
            ?.NotificationPreference.Should()
            .BeEquivalentTo(new GetNotificationPreferenceResponse(true, 7));

        testResultContent?.GeneralPreference.Should().BeEquivalentTo(new GetGeneralPreferenceResponse(false));
    }

    [Test]
    public async Task Should_Return401_When_NoAccessTokenProvided()
    {
        // Arrange
        // Act
        HttpResponseMessage testResult = await _httpClient.PostAsync(
            new StringBuilder().Append("user-preferences/preferences").ToString(), null);

        // Assert
        testResult.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}