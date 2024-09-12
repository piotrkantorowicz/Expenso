namespace Expenso.Api.Tests.E2E.Greetings;

internal sealed class HelloUser : TestBase
{
    [Test]
    public async Task Should_ReturnExpectedValue_Always()
    {
        // Arrange
        _httpClient.SetFakeBearerToken(token: _claims);

        // Act
        HttpResponseMessage response = await _httpClient.GetAsync(requestUri: "/greetings/hello-user");

        // Assert
        AssertResponseOk(response: response);
        string? responseContent = await response.Content.ReadFromJsonAsync<string>();
        responseContent.Should().Be(expected: $"Hello {Username}, I'm Expenso API");
    }

    [Test]
    public async Task Should_Return401__When_NoAccessTokenProvided()
    {
        // Arrange
        // Act
        HttpResponseMessage response = await _httpClient.GetAsync(requestUri: "/greetings/hello-user");

        // Assert
        AssertResponseUnauthroised(response: response);
    }
}