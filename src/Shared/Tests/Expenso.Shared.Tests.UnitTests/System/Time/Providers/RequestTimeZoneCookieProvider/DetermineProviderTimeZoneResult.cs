using Expenso.Shared.System.Time.Providers;

using Moq;

using NUnit.Framework;

using Shouldly;

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
        ProviderResult result = await TestCandidate.DetermineProviderResult(httpContext: _httpContextMock.Object,
            providerInput: _providerInputMock.Object);

        // Assert
        result.Value.ShouldBe(expected: "Pacific Standard Time");
    }

    [Test]
    public async Task Should_ReturnDefaultTimeZone_When_CookieDoesNotExist()
    {
        // Arrange
        _cookiesMock
            .Setup(expression: c => c.TryGetValue(It.IsAny<string>(), out It.Ref<string>.IsAny!))
            .Returns(value: false);

        // Act
        ProviderResult result = await TestCandidate.DetermineProviderResult(httpContext: _httpContextMock.Object,
            providerInput: _providerInputMock.Object);

        // Assert
        result.Value.ShouldBeNull();
    }
}