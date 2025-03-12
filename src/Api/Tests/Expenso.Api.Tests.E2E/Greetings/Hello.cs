using System.Net;
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
        responseContent.ShouldBe(expected: "Hello, I'm Expenso API");
        AssertResponseStatusCode(response: response, statusCode: HttpStatusCode.OK);
        AssertCorrelationIdHeader(response: response);
        AssertNoModuleHeader(response: response);
    }
}