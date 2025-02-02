using System.Net;
using System.Net.Http.Json;

using Expenso.Api.Tests.E2E.TestData;

using NUnit.Framework;

using Shouldly;

namespace Expenso.Api.Tests.E2E.Greetings;

[TestFixture]
internal sealed class HelloUser : TestBase
{
    [Test]
    public async Task Should_ReturnExpectedValue_Always()
    {
        // Arrange
        _httpClient.SetFakeBearerToken(token: Claims);

        // Act
        HttpResponseMessage response = await _httpClient.GetAsync(requestUri: "/greetings/hello-user");

        // Assert
        string? responseContent = await response.Content.ReadFromJsonAsync<string>();
        responseContent.ShouldBe(expected: $"Hello {TestClient.ClientName}, I'm Expenso API");
        AssertResponseStatusCode(response: response, statusCode: HttpStatusCode.OK);
        AssertCorrelationIdHeader(response: response);
        AssertNoModuleHeader(response: response);
    }

    [Test]
    public async Task Should_Return401__When_NoAccessTokenProvided()
    {
        // Arrange
        // Act
        HttpResponseMessage response = await _httpClient.GetAsync(requestUri: "/greetings/hello-user");

        // Assert
        AssertResponseStatusCode(response: response, statusCode: HttpStatusCode.Unauthorized);
        AssertNoCorrelationIdModuleHeader(response: response);
        AssertNoModuleHeader(response: response);
    }
}