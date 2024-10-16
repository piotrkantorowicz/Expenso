using Expenso.Shared.System.Types.TypesExtensions.Validations;

namespace Expenso.Shared.Tests.UnitTests.System.Types.TypesExtensions.Validations.StringExtensions;

[TestFixture]
internal sealed class IsValidHost
{
    [Test, TestCase(arg: "www.google.com"), TestCase(arg: "127.0.0.1"), TestCase(arg: "::1")]
    public void Should_ReturnTrue_When_HostIsValid(string host)
    {
        // Arrange
        // Act
        bool result = host.IsValidHost();

        // Assert
        result.Should().BeTrue();
    }

    [Test, TestCase(arguments: null), TestCase(arg: ""), TestCase(arg: "www.google..com"),
     TestCase(arg: "www google.com")]
    public void Should_ReturnFalse_When_HostIsInvalid(string host)
    {
        // Arrange
        // Act
        bool result = host.IsValidHost();

        // Assert
        result.Should().BeFalse();
    }
}