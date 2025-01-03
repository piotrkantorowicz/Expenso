using Expenso.Shared.System.Time.Constants;
using Expenso.Shared.System.Time.Providers;

using FluentAssertions;

using Moq;

using NUnit.Framework;

namespace Expenso.Shared.Tests.UnitTests.System.Time.Providers.RequestTimeZoneCookieProvider;

[TestFixture]
internal sealed class DetermineProviderTimeZoneResult : RequestTimeZoneCookieProviderTestBase
{
    [Test]
    public async Task Should_ReturnTimeZone_When_CookieExists()
    {
        // Arrange
        _cookiesMock
            .Setup(expression: c => c.TryGetValue(It.IsAny<string>(), out It.Ref<string>.IsAny!))
            .Returns(valueFunction: (string _, out string value) =>
            {
                value = "timezone=Pacific Standard Time";

                return true;
            });

        // Act
        ProviderTimeZoneResult? result =
            await TestCandidate.DetermineProviderTimeZoneResult(httpContext: _httpContextMock.Object);

        // Assert
        result.TimeZoneName.Should().Be(expected: "Pacific Standard Time");
    }

    [Test]
    public async Task Should_ReturnDefaultTimeZone_When_CookieDoesNotExist()
    {
        // Arrange
        _cookiesMock
            .Setup(expression: c => c.TryGetValue(It.IsAny<string>(), out It.Ref<string>.IsAny!))
            .Returns(value: false);

        // Act
        ProviderTimeZoneResult? result =
            await TestCandidate.DetermineProviderTimeZoneResult(httpContext: _httpContextMock.Object);

        // Assert
        result.TimeZoneName.Should().Be(expected: TimeZoneIds.Utc);
    }
}