using Expenso.Shared.System.Types.TypesExtensions;

using NUnit.Framework;

using Shouldly;

namespace Expenso.Shared.Tests.UnitTests.System.Types.TypesExtensions.DictionaryExtensions;

[TestFixture]
internal sealed class Merge
{
    [Test]
    public void Should_MergeDictionaries_When_NoConflictingKeys()
    {
        // Arrange
        Dictionary<int, string> dictionary = new()
        {
            { 1, "One" },
            { 2, "Two" }
        };

        Dictionary<int, string> itemsToMerge = new()
        {
            { 3, "Three" },
            { 4, "Four" }
        };

        // Act
        dictionary.Merge(items: itemsToMerge);

        // Assert
        dictionary.Count.ShouldBe(expected: 4);
        dictionary.ShouldContainKey(key: 3);
        dictionary.ShouldContainKey(key: 4);
        dictionary[key: 3].ShouldBe(expected: "Three");
        dictionary[key: 4].ShouldBe(expected: "Four");
    }

    [Test]
    public void Should_OverwriteValues_When_KeysConflictAndOverwriteIsTrue()
    {
        // Arrange
        Dictionary<int, string> dictionary = new()
        {
            { 1, "One" },
            { 2, "Two" }
        };

        Dictionary<int, string> itemsToMerge = new()
        {
            { 2, "Twenty" },
            { 3, "Three" }
        };

        // Act
        dictionary.Merge(items: itemsToMerge, overwrite: true);

        // Assert
        dictionary.Count.ShouldBe(expected: 3);
        dictionary[key: 2].ShouldBe(expected: "Twenty");
        dictionary[key: 3].ShouldBe(expected: "Three");
    }

    [Test]
    public void Should_ThrowInvalidOperationException_When_KeysConflictAndOverwriteIsFalse()
    {
        // Arrange
        Dictionary<int, string> dictionary = new()
        {
            { 1, "One" },
            { 2, "Two" }
        };

        Dictionary<int, string> itemsToMerge = new()
        {
            { 2, "Twenty" },
            { 3, "Three" }
        };

        // Act
        Action action = () => dictionary.Merge(items: itemsToMerge, overwrite: false);

        // Assert
        InvalidOperationException? exception = action.ShouldThrow<InvalidOperationException>();
        exception.Message.ShouldBe(expected: "Key '2' already exists in the dictionary and overwrite is not allowed.");
    }

    [Test]
    public void Should_ThrowArgumentNullException_When_DictionaryIsNull()
    {
        // Arrange
        IDictionary<int, string>? dictionary = null;

        Dictionary<int, string> itemsToMerge = new()
        {
            { 1, "One" }
        };

        // Act
        Action action = () => dictionary!.Merge(items: itemsToMerge);

        // Assert
        ArgumentNullException? exception = action.ShouldThrow<ArgumentNullException>();
        exception.Message.ShouldContain(expected: "dictionary");
    }

    [Test]
    public void Should_ThrowArgumentNullException_When_ItemsToMergeIsNull()
    {
        // Arrange
        Dictionary<int, string> dictionary = new()
        {
            { 1, "One" }
        };

        IDictionary<int, string>? itemsToMerge = null;

        // Act
        Action action = () => dictionary.Merge(items: itemsToMerge!);

        // Assert
        ArgumentNullException? exception = action.ShouldThrow<ArgumentNullException>();
        exception.Message.ShouldContain(expected: "items");
    }
}