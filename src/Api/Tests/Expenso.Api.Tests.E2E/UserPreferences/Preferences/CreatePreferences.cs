using System.Net;
using System.Net.Http.Json;

using Expenso.UserPreferences.Shared.DTO.API.CreatePreference.Request;
using Expenso.UserPreferences.Shared.DTO.API.CreatePreference.Response;

using FluentAssertions;

using NUnit.Framework;

namespace Expenso.Api.Tests.E2E.UserPreferences.Preferences;

[TestFixture]
internal sealed class CreatePreferences : PreferencesTestBase
{
    [Test]
    public async Task Should_ReturnExpectedResult()
    {
        // Arrange
        _httpClient.SetFakeBearerToken(token: _claims);
        Guid userId = Guid.NewGuid();
        Guid preferenceId = Guid.NewGuid();
        const string requestPath = "user-preferences/preferences";

        // Act
        HttpResponseMessage response = await _httpClient.PostAsJsonAsync(requestUri: requestPath,
            value: new CreatePreferenceRequest(PreferenceId: preferenceId, UserId: userId));

        // Assert
        AssertResponseCreated(response: response);

        CreatePreferenceResponse? responseContent =
            await response.Content.ReadFromJsonAsync<CreatePreferenceResponse>();

        responseContent.Should().NotBeNull();
        responseContent?.PreferenceId.Should().Be(expected: preferenceId);
    }

    [Test]
    public async Task Should_Return401_When_NoAccessTokenProvided()
    {
        // Arrange
        const string requestPath = "user-preferences/preferences";

        // Act
        HttpResponseMessage response = await _httpClient.PostAsync(requestUri: requestPath, content: null);

        // Assert
        AssertResponseUnauthroised(response: response);
    }
}