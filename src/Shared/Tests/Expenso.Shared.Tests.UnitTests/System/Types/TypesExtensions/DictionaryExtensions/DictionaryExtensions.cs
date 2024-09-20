using Expenso.Shared.System.Types.TypesExtensions;

namespace Expenso.Shared.Tests.UnitTests.System.Types.TypesExtensions.DictionaryExtensions;

[TestFixture]
internal sealed class DictionaryExtensionsTests
{
    [Test]
    public void Should_MergeDictionaries_When_NoConflictingKeys()
    {
        // Arrange
        Dictionary<int, string> dictionary = new Dictionary<int, string>
        {
            { 1, "One" },
            { 2, "Two" }
        };

        Dictionary<int, string> itemsToMerge = new Dictionary<int, string>
        {
            { 3, "Three" },
            { 4, "Four" }
        };

        // Act
        dictionary.Merge(items: itemsToMerge);

        // Assert
        dictionary.Should().HaveCount(expected: 4);
        dictionary.Should().ContainKey(expected: 3).And.ContainKey(expected: 4);
        dictionary[key: 3].Should().Be(expected: "Three");
        dictionary[key: 4].Should().Be(expected: "Four");
    }

    [Test]
    public void Should_OverwriteValues_When_KeysConflictAndOverwriteIsTrue()
    {
        // Arrange
        Dictionary<int, string> dictionary = new Dictionary<int, string>
        {
            { 1, "One" },
            { 2, "Two" }
        };

        Dictionary<int, string> itemsToMerge = new Dictionary<int, string>
        {
            { 2, "Twenty" },
            { 3, "Three" }
        };

        // Act
        dictionary.Merge(items: itemsToMerge, overwrite: true);

        // Assert
        dictionary.Should().HaveCount(expected: 3);
        dictionary[key: 2].Should().Be(expected: "Twenty");
        dictionary[key: 3].Should().Be(expected: "Three");
    }

    [Test]
    public void Should_ThrowInvalidOperationException_When_KeysConflictAndOverwriteIsFalse()
    {
        // Arrange
        Dictionary<int, string> dictionary = new Dictionary<int, string>
        {
            { 1, "One" },
            { 2, "Two" }
        };

        Dictionary<int, string> itemsToMerge = new Dictionary<int, string>
        {
            { 2, "Twenty" },
            { 3, "Three" }
        };

        // Act
        Action action = () => dictionary.Merge(items: itemsToMerge, overwrite: false);

        // Assert
        action
            .Should()
            .Throw<InvalidOperationException>()
            .WithMessage(
                expectedWildcardPattern: "Key '2' already exists in the dictionary and overwrite is not allowed.");
    }

    [Test]
    public void Should_ThrowArgumentNullException_When_DictionaryIsNull()
    {
        // Arrange
        IDictionary<int, string>? dictionary = null;

        Dictionary<int, string> itemsToMerge = new Dictionary<int, string>
        {
            { 1, "One" }
        };

        // Act
        Action action = () => dictionary!.Merge(items: itemsToMerge);

        // Assert
        action.Should().Throw<ArgumentNullException>().WithMessage(expectedWildcardPattern: "*dictionary*");
    }

    [Test]
    public void Should_ThrowArgumentNullException_When_ItemsToMergeIsNull()
    {
        // Arrange
        Dictionary<int, string> dictionary = new Dictionary<int, string>
        {
            { 1, "One" }
        };

        IDictionary<int, string>? itemsToMerge = null;

        // Act
        Action action = () => dictionary.Merge(items: itemsToMerge!);

        // Assert
        action.Should().Throw<ArgumentNullException>().WithMessage(expectedWildcardPattern: "*items*");
    }
}