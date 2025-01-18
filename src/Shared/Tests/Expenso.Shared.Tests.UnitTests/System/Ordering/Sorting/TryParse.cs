using Expenso.Shared.System.Types.Ordering;

using FluentAssertions;

using NUnit.Framework;

namespace Expenso.Shared.Tests.UnitTests.System.Ordering.Sorting;

internal sealed class TryParse
{
    [Test]
    public void Should_ReturnFalse_When_ValueIsNull()
    {
        // Arrange
        string? value = null;

        // Act
        bool result = Shared.System.Types.Ordering.Sorting.TryParse(value: value,
            sorting: out Shared.System.Types.Ordering.Sorting? sorting);

        // Assert
        result.Should().BeFalse();
        sorting.Should().BeNull();
    }

    [Test]
    public void Should_ReturnFalse_When_ValueIsEmpty()
    {
        // Arrange
        string value = string.Empty;

        // Act
        bool result = Shared.System.Types.Ordering.Sorting.TryParse(value: value,
            sorting: out Shared.System.Types.Ordering.Sorting? sorting);

        // Assert
        result.Should().BeFalse();
        sorting.Should().BeNull();
    }

    [Test]
    public void Should_ReturnFalse_When_ValueIsInvalid()
    {
        // Arrange
        const string value = "InvalidValue";

        // Act
        bool result = Shared.System.Types.Ordering.Sorting.TryParse(value: value,
            sorting: out Shared.System.Types.Ordering.Sorting? sorting);

        // Assert
        result.Should().BeFalse();
        sorting.Should().BeNull();
    }

    [Test]
    public void Should_ReturnFalse_When_SortOrderIsInvalid()
    {
        // Arrange
        const string value = "Name:InvalidOrder";

        // Act
        bool result = Shared.System.Types.Ordering.Sorting.TryParse(value: value,
            sorting: out Shared.System.Types.Ordering.Sorting? sorting);

        // Assert
        result.Should().BeFalse();
        sorting.Should().BeNull();
    }

    [Test]
    public void Should_ReturnFalse_When_SortByIsEmpty()
    {
        // Arrange
        const string value = ":Ascending";

        // Act
        bool result = Shared.System.Types.Ordering.Sorting.TryParse(value: value,
            sorting: out Shared.System.Types.Ordering.Sorting? sorting);

        // Assert
        result.Should().BeFalse();
        sorting.Should().BeNull();
    }

    [Test]
    public void Should_ReturnTrue_When_ValueIsValid()
    {
        // Arrange
        const string value = "Name:Ascending,Id:Descending";

        // Act
        bool result = Shared.System.Types.Ordering.Sorting.TryParse(value: value,
            sorting: out Shared.System.Types.Ordering.Sorting? sorting);

        // Assert
        result.Should().BeTrue();
        sorting.Should().NotBeNull();
        sorting!.Sorters.Should().HaveCount(expected: 2);

        sorting
            .Sorters.Should()
            .ContainSingle(predicate: s => s.SortBy == "Name" && s.SortOrder == SortOrder.Ascending);

        sorting.Sorters.Should().ContainSingle(predicate: s => s.SortBy == "Id" && s.SortOrder == SortOrder.Descending);
    }

    [Test]
    public void Should_ReturnTrue_When_ValueHasExtraSpaces()
    {
        // Arrange
        const string value = "  Name  :  Ascending  ,  Id  :  Descending  ";

        // Act
        bool result = Shared.System.Types.Ordering.Sorting.TryParse(value: value,
            sorting: out Shared.System.Types.Ordering.Sorting? sorting);

        // Assert
        result.Should().BeTrue();
        sorting.Should().NotBeNull();
        sorting!.Sorters.Should().HaveCount(expected: 2);

        sorting
            .Sorters.Should()
            .ContainSingle(predicate: s => s.SortBy == "Name" && s.SortOrder == SortOrder.Ascending);

        sorting.Sorters.Should().ContainSingle(predicate: s => s.SortBy == "Id" && s.SortOrder == SortOrder.Descending);
    }

    [Test]
    public void Should_ReturnTrue_When_ValueHasSingleSorter()
    {
        // Arrange
        const string value = "Name:Ascending";

        // Act
        bool result = Shared.System.Types.Ordering.Sorting.TryParse(value: value,
            sorting: out Shared.System.Types.Ordering.Sorting? sorting);

        // Assert
        result.Should().BeTrue();
        sorting.Should().NotBeNull();
        sorting!.Sorters.Should().HaveCount(expected: 1);

        sorting
            .Sorters.Should()
            .ContainSingle(predicate: s => s.SortBy == "Name" && s.SortOrder == SortOrder.Ascending);
    }
}