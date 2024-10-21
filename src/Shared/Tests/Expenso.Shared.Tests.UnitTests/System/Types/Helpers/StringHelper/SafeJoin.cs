namespace Expenso.Shared.Tests.UnitTests.System.Types.Helpers.StringHelper;

[TestFixture]
internal sealed class SafeJoin
{
    [Test]
    public void Should_ReturnEmptyString_When_ValuesIsNull()
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
    public void Should_ReturnEmptyString_When_ValuesIsEmpty()
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
    public void Should_ShouldOmitNullValues_When_ValuesContainsNull()
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
    public void Should_ShouldOmitEmptyStrings_When_ValuesContainsEmptyString()
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
    public void Should_ShouldOmitWhitespaceStrings_When_ValuesContainsWhitespace()
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
    public void Should_ShouldOmitInvalidValues_When_ValuesContainsNullAndEmptyStringAndWhitespace()
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