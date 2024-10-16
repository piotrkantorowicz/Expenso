using Expenso.Shared.System.Types.TypesExtensions.Validations;

namespace Expenso.Shared.Tests.UnitTests.System.Types.TypesExtensions.Validations.StringExtensions;

[TestFixture]
internal sealed class IsValidEmail
{
    [Test]
    public void Should_ReturnTrue_When_EmailIsValid()
    {
        // Arrange
        const string? email = "lyubov.ray@email.com";

        // Act
        bool result = email.IsValidEmail();

        // Assert
        result.Should().BeTrue();
    }

    [Test, TestCase(arguments: null), TestCase(arg: ""), TestCase(arg: "lyubov.rayemail.com"),
     TestCase(arg: "lyubov.ray@.com"), TestCase(arg: "lyubov ray@email.com")]
    public void Should_ReturnFalse_When_EmailIsInvalid(string email)
    {
        // Arrange
        // Act
        bool result = email.IsValidEmail();

        // Assert
        result.Should().BeFalse();
    }
}