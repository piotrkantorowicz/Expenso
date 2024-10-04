using Expenso.Shared.System.Types.TypesExtensions.Validations;

namespace Expenso.Shared.Tests.UnitTests.System.Types.TypesExtensions.Validations.IntExtensions;

internal sealed class IsValidPort
{
    [Test]
    public void Should_ReturnTrue_When_PortIsInRange()
    {
        // Arrange
        int? port = 1234;

        // Act
        bool result = port.IsValidPort();

        // Assert
        result.Should().BeTrue();
    }

    [Test]
    public void Should_ReturnFalse_When_PortIsNull()
    {
        // Arrange
        int? port = null;

        // Act
        bool result = port.IsValidPort();

        // Assert
        result.Should().BeFalse();
    }

    [Test]
    public void Should_ReturnFalse_When_PortIsLessThanOne()
    {
        // Arrange
        int? port = 0;

        // Act
        bool result = port.IsValidPort();

        // Assert
        result.Should().BeFalse();
    }

    [Test]
    public void Should_ReturnFalse_When_PortIsGreaterThan65535()
    {
        // Arrange
        int? port = 65536;

        // Act
        bool result = port.IsValidPort();

        // Assert
        result.Should().BeFalse();
    }
}