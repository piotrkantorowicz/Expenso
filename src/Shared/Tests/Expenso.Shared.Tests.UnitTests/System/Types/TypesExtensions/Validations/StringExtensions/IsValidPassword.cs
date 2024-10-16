using Expenso.Shared.System.Types.TypesExtensions.Validations;

namespace Expenso.Shared.Tests.UnitTests.System.Types.TypesExtensions.Validations.StringExtensions;

[TestFixture]
internal sealed class IsValidPassword
{
    [Test]
    public void Should_ReturnTrue_When_PasswordIsValid()
    {
        // Arrange
        const string? password = "Password123!";

        // Act
        bool result = password.IsValidPassword();

        // Assert
        result.Should().BeTrue();
    }

    [Test, TestCase(arguments: null), TestCase(arg: ""), TestCase(arg: "password123!"), TestCase(arg: "PASSWORD123!"),
     TestCase(arg: "Password!"), TestCase(arg: "asdafFSsdrdD23"), TestCase(arg: "adasdsdfsdasdaaw"),
     TestCase(arg: "FHYEDSACSDF"), TestCase(arg: "1234567890"), TestCase(arg: "Pass1!@")]
    public void Should_ReturnFalse_When_PasswordIsInvalid(string password)
    {
        // Arrange
        // Act
        bool result = password.IsValidPassword();

        // Assert
        result.Should().BeFalse();
    }
}