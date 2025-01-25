using Expenso.Shared.System.Time.Providers;

using Microsoft.Extensions.Primitives;

using Moq;

using NUnit.Framework;

using Shouldly;

namespace Expenso.Shared.Tests.UnitTests.System.Time.Providers.RequestTimeZoneQueryStringProvider;

[TestFixture]
internal sealed class DetermineProviderTimeZoneResult : RequestTimeZoneQueryStringProviderTestBase
{
    [Test]
    public async Task Should_ReturnTimeZone_When_QueryStringExists()
    {
        // Arrange
        _httpRequestMock
            .Setup(expression: r => r.Query.TryGetValue(It.IsAny<string>(), out It.Ref<StringValues>.IsAny))
            .Returns(valueFunction: (string key, out StringValues value) =>
            {
                value = "TimeZone=Pacific Standard Time";

                return true;
            });

        // Act
        ProviderTimeZoneResult result =
            await TestCandidate.DetermineProviderTimeZoneResult(httpContext: _httpContextMock.Object);

        // Assert
        result.TimeZoneName.ShouldBe(expected: "Pacific Standard Time");
    }

    [Test]
    public async Task Should_ReturnDefaultTimeZone_When_QueryStringDoesNotExist()
    {
        // Arrange
        _httpRequestMock
            .Setup(expression: r => r.Query.TryGetValue(It.IsAny<string>(), out It.Ref<StringValues>.IsAny))
            .Returns(value: false);

        // Act
        ProviderTimeZoneResult result =
            await TestCandidate.DetermineProviderTimeZoneResult(httpContext: _httpContextMock.Object);

        // Assert
        result.TimeZoneName.ShouldBeNull();
    }
}