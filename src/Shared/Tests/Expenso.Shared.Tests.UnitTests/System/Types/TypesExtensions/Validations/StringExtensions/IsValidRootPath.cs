using Expenso.Shared.System.Types.TypesExtensions.Validations;

namespace Expenso.Shared.Tests.UnitTests.System.Types.TypesExtensions.Validations.StringExtensions;

internal sealed class IsValidRootPath
{
    [Test, TestCase(arg: "/")]
    public void Should_ReturnTrue_When_RootPathIsValid(string rootPath)
    {
        // Arrange
        // Act
        bool result = rootPath.IsValidRootPath();

        // Assert
        result.Should().BeTrue();
    }

    [Test, TestCase(arguments: null), TestCase(arg: ""), TestCase(arg: "C"), TestCase(arg: "C:")]
    public void Should_ReturnFalse_When_RootPathIsInvalid(string rootPath)
    {
        // Arrange
        // Act
        bool result = rootPath.IsValidRootPath();

        // Assert
        result.Should().BeFalse();
    }
}