using Expenso.Api.Configuration.Options;

namespace Expenso.Api.Tests.UnitTests.Configuration.Options.Cases;

internal sealed class TryBindOptions : OptionsExtensionsTestBase
{
    [Test]
    public void Should_ReturnTrue_AndAssignProperties_WhenConfigurationIsValid()
    {
        // Arrange
        // Act
        bool testResult = Configuration.TryBindOptions("MyOptions", out MyOptions options);

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
        bool testResult = Configuration.TryBindOptions("MyOptions1", out MyOptions options);

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
        bool testResult = Configuration.TryBindOptions(null, out MyOptions options);
#pragma warning restore CS8625

        // Assert
        testResult.Should().BeFalse();
    }
}