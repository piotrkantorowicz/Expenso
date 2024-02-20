using System.Text;

using Expenso.UserPreferences.Core.Application.Preferences.Write.Commands.CreatePreference.Internal.DTO.Request;

namespace Expenso.Api.Tests.E2E.UserPreferences.Preferences;

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
        Guid? testResultContent = await testResult.Content.ReadFromJsonAsync<Guid>();
        testResultContent.Should().NotBeEmpty();
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