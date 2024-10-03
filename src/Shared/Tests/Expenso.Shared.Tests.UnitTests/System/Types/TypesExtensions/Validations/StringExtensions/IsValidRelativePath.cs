using Expenso.Shared.System.Types.TypesExtensions.Validations;

namespace Expenso.Shared.Tests.UnitTests.System.Types.TypesExtensions.Validations.StringExtensions;

internal sealed class IsValidRelativePath
{
    [Test]
    public void Should_ReturnTrue_When_PathIsValid()
    {
        // Arrange
        const string path = @"Users\lyubovray\Desktop\folder\file.txt\file.txt";

        // Act
        bool result = path.IsValidRelativePath();

        // Assert
        result.Should().BeTrue();
    }

    [Test, TestCase(arguments: null), TestCase(arg: ""), TestCase(arg: @"C:\Users\lyubovray\Desktop\folder\file.txt\"),
     TestCase(arg: @"\Users\lyubovray\Desktop\folder\file.txt\file.txt")]
    public void Should_ReturnFalse_When_PathIsInvalid(string path)
    {
        // Arrange
        // Act
        bool result = path.IsValidRelativePath();

        // Assert
        result.Should().BeFalse();
    }
}