using Expenso.Shared.System.Types.TypesExtensions.Validations;

namespace Expenso.Shared.Tests.UnitTests.System.Types.TypesExtensions.Validations.StringExtensions;

internal sealed class IsAlphaString
{
    [Test, TestCase(arg: "abc"), TestCase(arg: "ABC"), TestCase(arg: "aBc")]
    public void Should_ReturnTrue_When_StringIsAlpha(string str)
    {
        // Arrange
        // Act
        bool result = str.IsAlphaString();

        // Assert
        result.Should().BeTrue();
    }

    [Test, TestCase(arguments: null), TestCase(arg: ""), TestCase(arg: "abc1"), TestCase(arg: "abc!")]
    public void Should_ReturnFalse_When_StringIsNotAlpha(string str)
    {
        // Arrange
        // Act
        bool result = str.IsAlphaString();

        // Assert
        result.Should().BeFalse();
    }

    [Test, TestCase(arg: "ab"), TestCase(arg: "A"), TestCase(arg: "aBcasdDDDss")]
    public void Should_ReturnFalse_When_StringIsNotInRange(string str)
    {
        // Arrange
        // Act
        bool result = str.IsAlphaString(minLength: 3, maxLength: 10);

        // Assert
        result.Should().BeFalse();
    }
}