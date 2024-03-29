namespace Expenso.Api.Tests.E2E.Greetings;

internal sealed class Hello : GreetingsTestBase
{
    [Test]
    public async Task Should_ReturnExpectedValue_Always()
    {
        // Arrange
        // Act
        HttpResponseMessage testResult = await _httpClient.GetAsync("/greetings/hello");

        // Assert
        string? testResultContent = await testResult.Content.ReadFromJsonAsync<string>();
        testResult.StatusCode.Should().Be(HttpStatusCode.OK);
        testResultContent.Should().Be("Hello, I'm Expenso API.");
    }
}