using Expenso.Shared.System.Types.TypesExtensions.Validations;

namespace Expenso.Shared.Tests.UnitTests.System.Types.TypesExtensions.Validations.StringExtensions;

[TestFixture]
internal sealed class IsAlphaString
{
    [Test, TestCase(arg: "abc"), TestCase(arg: "ABC"), TestCase(arg: "aBc")]
    public void Should_ReturnTrue_When_StringIsAlpha(string target)
    {
        // Arrange
        // Act
        bool result = target.IsAlphaString();

        // Assert
        result.Should().BeTrue();
    }

    [Test, TestCase(arg: null), TestCase(arg: ""), TestCase(arg: "   "), TestCase(arg: "abc1"), TestCase(arg: "abc!"),
     TestCase(arg: "123"), TestCase(arg: "@#$"), TestCase(arg: "a b c")]
    public void Should_ReturnFalse_When_StringIsNotAlpha(string target)
    {
        // Arrange
        // Act
        bool result = target.IsAlphaString();

        // Assert
        result.Should().BeFalse();
    }

    [Test, TestCase(arg: "ab"), TestCase(arg: "A"), TestCase(arg: "aBcasdDDDss")]
    public void Should_ReturnFalse_When_StringIsNotInRange(string target)
    {
        // Arrange
        // Act
        bool result = target.IsAlphaString(minLength: 3, maxLength: 10);

        // Assert
        result.Should().BeFalse();
    }
}