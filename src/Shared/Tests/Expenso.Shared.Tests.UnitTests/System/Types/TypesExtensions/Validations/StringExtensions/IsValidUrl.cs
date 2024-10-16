using Expenso.Shared.System.Types.TypesExtensions.Validations;

namespace Expenso.Shared.Tests.UnitTests.System.Types.TypesExtensions.Validations.StringExtensions;

[TestFixture]
internal sealed class IsValidUrl
{
    [Test, TestCase(arg: "http://www.google.com"), TestCase(arg: "https://www.google.com"),
     TestCase(arg: "http://localhost:5000")]
    public void Should_ReturnTrue_When_UrlIsValid(string url)
    {
        // Arrange
        // Act
        bool result = url.IsValidUrl();

        // Assert
        result.Should().BeTrue();
    }

    [Test, TestCase(arguments: null), TestCase(arg: ""), TestCase(arg: "www.google.com"),
     TestCase(arg: "http://www.google..com"), TestCase(arg: "http://www google.com")]
    public void Should_ReturnFalse_When_UrlIsInvalid(string url)
    {
        // Arrange
        // Act
        bool result = url.IsValidUrl();

        // Assert
        result.Should().BeFalse();
    }
}