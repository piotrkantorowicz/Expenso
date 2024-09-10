namespace Expenso.Api.Tests.E2E.Greetings;

internal sealed class HelloUser : GreetingsTestBase
{
    [Test]
    public async Task Should_ReturnExpectedValue_Always()
    {
        // Arrange
        _httpClient.SetFakeBearerToken(token: _claims);

        // Act
        HttpResponseMessage response = await _httpClient.GetAsync(requestUri: "/greetings/hello-user");

        // Assert
        response.StatusCode.Should().Be(expected: HttpStatusCode.OK);
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