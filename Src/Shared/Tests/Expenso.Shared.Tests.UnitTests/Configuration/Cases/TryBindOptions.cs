using Expenso.Shared.Configuration.Extensions;

namespace Expenso.Shared.Tests.UnitTests.Configuration.Cases;

internal sealed class TryBindOptions : OptionsExtensionsTestBase
{
    [Test]
    public void Should_ReturnTrue_AndAssignProperties_WhenConfigurationIsValid()
    {
        // Arrange
        // Act
        bool testResult = TestCandidate.TryBindOptions("MyOptions", out MyOptions options);

        // Assert
        testResult.Should().BeTrue();
        options.Option1.Should().Be("Option1 value");
        options.Option2.Should().Be(500);
    }

    [Test]
    public void Should_ReturnTrue_AndEmptyOptions_WhenWrongSectionNameProvided()
    {
        // Arrange
        // Act
        bool testResult = TestCandidate.TryBindOptions("MyOptions1", out MyOptions options);

        // Assert
        testResult.Should().BeTrue();
        options.Option1.Should().Be(null);
        options.Option2.Should().Be(0);
    }

    [Test]
    public void Should_ReturnFalse_AndEmptyOptions_SomethingThrown()
    {
        // Arrange
        // Act
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
        bool testResult = TestCandidate.TryBindOptions(null, out MyOptions _);
#pragma warning restore CS8625

        // Assert
        testResult.Should().BeFalse();
    }
}