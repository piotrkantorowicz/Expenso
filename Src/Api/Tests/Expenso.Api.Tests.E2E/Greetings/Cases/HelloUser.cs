namespace Expenso.Api.Tests.E2E.Greetings.Cases;

internal sealed class HelloUser : GreetingsTestBase
{
    /* TODO: Temporary commented out until setup keycloak at github actions
    [Test]
    */
    public async Task Should_ReturnExpectedValue_Always()
    {
        // Arrange
        HttpClient.SetBearerToken(TestAuth.Token);

        // Act
        HttpResponseMessage testResult = await HttpClient.GetAsync("/greetings/hello-user");

        // Assert
        string? testResultContent = await testResult.Content.ReadFromJsonAsync<string>();
        testResult.StatusCode.Should().Be(HttpStatusCode.OK);
        testResultContent.Should().Be($"Hello {TestAuth.TestUsername}, I'm Expenso API.");
    }

    [Test]
    public async Task Should_Return401_WhenAuthFailed()
    {
        // Arrange
        // Act
        HttpResponseMessage testResult = await HttpClient.GetAsync("/greetings/hello-user");

        // Assert
        testResult.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}