namespace Expenso.Api.Tests.E2E.Greetings;

internal sealed class HelloUser : GreetingsTestBase
{
    [Test]
    public async Task Should_ReturnExpectedValue_Always()
    {
        // Arrange
        _httpClient.SetFakeBearerToken(token: _claims);

        // Act
        HttpResponseMessage testResult = await _httpClient.GetAsync(requestUri: "/greetings/hello-user");

        // Assert
        testResult.StatusCode.Should().Be(expected: HttpStatusCode.OK);
        string? testResultContent = await testResult.Content.ReadFromJsonAsync<string>();
        testResultContent.Should().Be(expected: $"Hello {Username}, I'm Expenso API");
    }

    [Test]
    public async Task Should_Return401__When_NoAccessTokenProvided()
    {
        // Arrange
        // Act
        HttpResponseMessage testResult = await _httpClient.GetAsync(requestUri: "/greetings/hello-user");

        // Assert
        testResult.StatusCode.Should().Be(expected: HttpStatusCode.Unauthorized);
    }
}