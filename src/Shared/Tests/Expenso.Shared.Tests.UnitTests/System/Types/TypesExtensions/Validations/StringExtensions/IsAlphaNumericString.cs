using Expenso.Shared.System.Types.TypesExtensions.Validations;

namespace Expenso.Shared.Tests.UnitTests.System.Types.TypesExtensions.Validations.StringExtensions;

internal sealed class IsAlphaNumericString
{
    [Test, TestCase(arg: "lyubovray"), TestCase(arg: "lyubovray123")]
    public void Should_ReturnTrue_When_StringIsAlphaNumeric(string target)
    {
        // Arrange
        // Act
        bool result = target.IsAlphaNumericString();

        // Assert
        result.Should().BeTrue();
    }

    [Test, TestCase(arguments: null), TestCase(arg: ""), TestCase(arg: "lyub1v ray"), TestCase(arg: "lyubov.ray1")]
    public void Should_ReturnFalse_When_StringIsNotAlphaNumeric(string target)
    {
        // Arrange
        // Act
        bool result = target.IsAlphaNumericString();

        // Assert
        result.Should().BeFalse();
    }

    [Test, TestCase(arg: "a1"), TestCase(arg: "2"), TestCase(arg: "aBcasdDDD1s")]
    public void Should_ReturnFalse_When_StringIsNotInRange(string target)
    {
        // Arrange
        // Act
        bool result = target.IsAlphaNumericString(minLength: 3, maxLength: 10);

        // Assert
        result.Should().BeFalse();
    }
}