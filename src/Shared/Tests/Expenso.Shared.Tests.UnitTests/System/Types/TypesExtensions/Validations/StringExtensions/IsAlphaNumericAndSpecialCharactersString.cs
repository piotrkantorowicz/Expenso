using Expenso.Shared.System.Types.TypesExtensions.Validations;

namespace Expenso.Shared.Tests.UnitTests.System.Types.TypesExtensions.Validations.StringExtensions;

[TestFixture]
internal sealed class IsAlphaNumericAndSpecialCharactersString
{
    [Test]
    public void Should_ReturnTrue_When_StringIsAlphaNumericAndSpecialCharacters()
    {
        // Arrange
        const string target = "lyubovray123!@#";

        // Act
        bool result = target.IsAlphaNumericAndSpecialCharactersString();

        // Assert
        result.Should().BeTrue();
    }

    [Test, TestCase(arguments: null), TestCase(arg: ""), TestCase(arg: "   "), TestCase(arg: "lyubov ra@y1")]
    public void Should_ReturnFalse_When_StringIsNotAlphaNumericAndSpecialCharacters(string target)
    {
        // Arrange
        // Act
        bool result = target.IsAlphaNumericAndSpecialCharactersString();

        // Assert
        result.Should().BeFalse();
    }

    [Test, TestCase(arg: "a1"), TestCase(arg: "2"), TestCase(arg: "aBcasdDDD1s")]
    public void Should_ReturnFalse_When_StringIsNotInRange(string target)
    {
        // Arrange
        // Act
        bool result = target.IsAlphaNumericAndSpecialCharactersString(minLength: 3, maxLength: 10);

        // Assert
        result.Should().BeFalse();
    }
}