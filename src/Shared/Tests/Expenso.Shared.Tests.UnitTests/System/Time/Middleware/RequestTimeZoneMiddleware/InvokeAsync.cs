using Expenso.Shared.System.Time.Providers;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

using Moq;

using NUnit.Framework;

namespace Expenso.Shared.Tests.UnitTests.System.Time.Middleware.RequestTimeZoneMiddleware;

[TestFixture]
internal sealed class InvokeAsync : RequestTimeZoneMiddlewareTestBase
{
    [Test]
    public async Task Should_SetUtcRequestTimeZoneFeature_When_NoTimezoneHasBeenProvided()
    {
        // Arrange
        // Act
        await TestCandidate.InvokeAsync(httpContext: _httpContext, next: _nextMock.Object);

        // Assert
        AssertRequestTimeZoneFeature(timeZoneId: "UTC");
    }

    [Test]
    public async Task Should_SetRequestTimeZoneFeature_When_ValidTimeZoneProvidedFromCookies()
    {
        // Arrange
        Mock<IRequestCookieCollection> cookieCollection = new();
        string? cookieValue = "Pacific Standard Time";

        cookieCollection
            .Setup(expression: c => c.TryGetValue(".AspNetCore.TimeZone", out cookieValue))
            .Returns(value: true);

        _httpContext.Request.Cookies = cookieCollection.Object;

        // Act
        await TestCandidate.InvokeAsync(httpContext: _httpContext, next: _nextMock.Object);

        // Assert
        AssertRequestTimeZoneFeature(timeZoneId: "Pacific Standard Time",
            providerType: typeof(RequestTimeZoneCookieProvider));
    }

    [Test]
    public async Task Should_SetRequestTimeZoneFeature_When_ValidTimeZoneProvidedFromQueryString()
    {
        // Arrange

        _httpContext.Request.Query = new QueryCollection(store: new Dictionary<string, StringValues>
        {
            { "TimeZone", new StringValues(value: "America/Anchorage") }
        });

        // Act
        await TestCandidate.InvokeAsync(httpContext: _httpContext, next: _nextMock.Object);

        // Assert
        AssertRequestTimeZoneFeature(timeZoneId: "America/Anchorage",
            providerType: typeof(RequestTimeZoneQueryStringProvider));
    }

    [Test]
    public async Task Should_SetRequestTimeZoneFeature_When_ValidTimeZoneProvidedFromHeader()
    {
        // Arrange
        _httpContext.Request.Headers.Add(item: new KeyValuePair<string, StringValues>(key: "Time-Zone",
            value: new StringValues(value: "Europe/Warsaw")));

        // Act
        await TestCandidate.InvokeAsync(httpContext: _httpContext, next: _nextMock.Object);

        // Assert
        AssertRequestTimeZoneFeature(timeZoneId: "Europe/Warsaw", providerType: typeof(RequestTimeZoneHeaderProvider));
    }
}