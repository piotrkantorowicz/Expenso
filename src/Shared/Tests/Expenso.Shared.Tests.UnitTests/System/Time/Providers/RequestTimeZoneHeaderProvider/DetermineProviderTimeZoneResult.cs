using Expenso.Shared.System.Time.Providers;

using Microsoft.Extensions.Primitives;

using Moq;

using NUnit.Framework;

using Shouldly;

namespace Expenso.Shared.Tests.UnitTests.System.Time.Providers.RequestTimeZoneHeaderProvider;

[TestFixture]
internal sealed class DetermineProviderTimeZoneResult : RequestTimeZoneHeaderProviderTestBase
{
    [Test]
    public async Task Should_ReturnTimeZone_When_HeaderExists()
    {
        // Arrange
        _httpRequestMock
            .Setup(expression: r => r.Headers.TryGetValue(It.IsAny<string>(), out It.Ref<StringValues>.IsAny))
            .Returns(valueFunction: (string _, out StringValues value) =>
            {
                value = "Time-Zone=Pacific Standard Time";

                return true;
            });

        // Act
        ProviderTimeZoneResult result =
            await TestCandidate.DetermineProviderTimeZoneResult(httpContext: _httpContextMock.Object);

        // Assert
        result.TimeZoneName.ShouldBe(expected: "Pacific Standard Time");
    }

    [Test]
    public async Task Should_ReturnDefaultTimeZone_When_HeaderDoesNotExist()
    {
        // Arrange
        _httpRequestMock
            .Setup(expression: r => r.Headers.TryGetValue(It.IsAny<string>(), out It.Ref<StringValues>.IsAny))
            .Returns(value: false);

        // Act
        ProviderTimeZoneResult result =
            await TestCandidate.DetermineProviderTimeZoneResult(httpContext: _httpContextMock.Object);

        // Assert
        result.TimeZoneName.ShouldBeNull();
    }
}