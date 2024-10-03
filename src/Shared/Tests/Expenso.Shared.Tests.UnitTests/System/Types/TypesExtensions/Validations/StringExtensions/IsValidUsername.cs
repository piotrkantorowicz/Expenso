using Expenso.Shared.System.Types.TypesExtensions.Validations;

namespace Expenso.Shared.Tests.UnitTests.System.Types.TypesExtensions.Validations.StringExtensions;

internal sealed class IsValidUsername
{
    [Test]
    public void Should_ReturnTrue_When_UsernameIsValid()
    {
        // Arrange
        const string? username = "lyubovray";

        // Act
        bool result = username.IsValidUsername();

        // Assert
        result.Should().BeTrue();
    }

    [Test, TestCase(arguments: null), TestCase(arg: ""), TestCase(arg: "ly"),
     TestCase(arg: "skfsdklhfgjsdkghamasdhbygwehbbsdnfklsdmfklsdfksdfjsdhnfjsdnfjksdfnsjkfnk"),
     TestCase(arg: "lyubov ray"), TestCase(arg: "lyubov.ray")]
    public void Should_ReturnFalse_When_UsernameIsInvalid(string username)
    {
        // Arrange
        // Act
        bool result = username.IsValidUsername();

        // Assert
        result.Should().BeFalse();
    }
}