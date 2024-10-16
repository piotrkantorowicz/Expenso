using Expenso.Shared.System.Configuration;

namespace Expenso.Shared.Tests.UnitTests.System.Configuration.Extensions;

[TestFixture]
internal sealed class TryBindOptions : OptionsExtensionsTestBase
{
    [Test]
    public void Should_ReturnTrue_AndAssignProperties_WhenConfigurationIsValid()
    {
        // Arrange
        // Act
        bool testResult = TestCandidate.TryBindOptions(sectionName: "MyOptions", options: out MyOptions options);

        // Assert
        testResult.Should().BeTrue();
        options.Option1.Should().Be(expected: "Option1 value");
        options.Option2.Should().Be(expected: 500);
    }

    [Test]
    public void Should_ReturnTrue_AndEmptyOptions_WhenWrongSectionNameProvided()
    {
        // Arrange
        // Act
        bool testResult = TestCandidate.TryBindOptions(sectionName: "MyOptions1", options: out MyOptions options);

        // Assert
        testResult.Should().BeTrue();
        options.Option1.Should().Be(expected: null);
        options.Option2.Should().Be(expected: 0);
    }

    [Test]
    public void Should_ReturnFalse_AndEmptyOptions_SomethingThrown()
    {
        // Arrange
        // Act
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
        bool testResult = TestCandidate.TryBindOptions(sectionName: null, options: out MyOptions _);
#pragma warning restore CS8625

        // Assert
        testResult.Should().BeFalse();
    }
}