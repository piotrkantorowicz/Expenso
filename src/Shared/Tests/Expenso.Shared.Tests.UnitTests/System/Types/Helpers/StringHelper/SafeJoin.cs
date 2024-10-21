namespace Expenso.Shared.Tests.UnitTests.System.Types.Helpers.StringHelper;

[TestFixture]
internal sealed class SafelyJoin
{
    [Test]
    public void SafelyJoin_WhenValuesIsNull_ShouldReturnEmptyString()
    {
        // Arrange
        const string? separator = " ";
        string?[]? values = null;

        // Act
        string result = Shared.System.Types.Helpers.StringHelper.SafelyJoin(separator: separator, values: values);

        // Assert
        result.Should().BeEmpty();
    }

    [Test]
    public void SafelyJoin_WhenValuesIsEmpty_ShouldReturnEmptyString()
    {
        // Arrange
        const string? separator = " ";
        string?[] values = [];

        // Act
        string result = Shared.System.Types.Helpers.StringHelper.SafelyJoin(separator: separator, values: values);

        // Assert
        result.Should().BeEmpty();
    }

    [Test]
    public void SafelyJoin_WhenValuesContainsNull_ShouldReturnEmptyString()
    {
        // Arrange
        const string? separator = " ";

        string?[] values =
        [
            "John",
            null,
            "Doe"
        ];

        // Act
        string result = Shared.System.Types.Helpers.StringHelper.SafelyJoin(separator: separator, values: values);

        // Assert
        result.Should().Be(expected: "John Doe");
    }

    [Test]
    public void SafelyJoin_WhenValuesContainsEmptyString_ShouldReturnEmptyString()
    {
        // Arrange
        const string separator = " ";

        string?[] values =
        [
            "John",
            string.Empty,
            "Doe"
        ];

        // Act
        string result = Shared.System.Types.Helpers.StringHelper.SafelyJoin(separator: separator, values: values);

        // Assert
        result.Should().Be(expected: "John Doe");
    }

    [Test]
    public void SafelyJoin_WhenValuesContainsWhitespace_ShouldReturnEmptyString()
    {
        // Arrange
        const string separator = " ";

        string?[] values =
        [
            "John",
            " ",
            "Doe"
        ];

        // Act
        string result = Shared.System.Types.Helpers.StringHelper.SafelyJoin(separator: separator, values: values);

        // Assert
        result.Should().Be(expected: "John Doe");
    }

    [Test]
    public void SafelyJoin_WhenValuesContainsNullAndEmptyStringAndWhitespace_ShouldReturnEmptyString()
    {
        // Arrange
        const string separator = " ";

        string?[] values =
        [
            "John",
            null,
            string.Empty,
            " ",
            "Doe"
        ];

        // Act
        string result = Shared.System.Types.Helpers.StringHelper.SafelyJoin(separator: separator, values: values);

        // Assert
        result.Should().Be(expected: "John Doe");
    }
}