using System.Net.Http.Json;

using NUnit.Framework;

using Shouldly;

namespace Expenso.Api.Tests.E2E.Greetings;

[TestFixture]
internal sealed class Hello : TestBase
{
    [Test]
    public async Task Should_ReturnExpectedValue_Always()
    {
        // Arrange
        // Act
        HttpResponseMessage response = await _httpClient.GetAsync(requestUri: "/greetings/hello");

        // Assert
        string? responseContent = await response.Content.ReadFromJsonAsync<string>();
        AssertResponseOk(response: response);
        responseContent.ShouldBe(expected: "Hello, I'm Expenso API");
    }
}