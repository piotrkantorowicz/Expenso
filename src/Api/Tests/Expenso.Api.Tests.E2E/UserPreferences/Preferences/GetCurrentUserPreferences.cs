using System.Net;
using System.Net.Http.Json;

using Expenso.Api.Tests.E2E.TestData.Preferences;
using Expenso.UserPreferences.Core.Application.Preferences.Read.Queries.GetPreference.DTO.Response;

using FluentAssertions;

using NUnit.Framework;

namespace Expenso.Api.Tests.E2E.UserPreferences.Preferences;

[TestFixture]
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
        AssertResponseOk(response: response);
        GetPreferenceResponse? responseContent = await response.Content.ReadFromJsonAsync<GetPreferenceResponse>();
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