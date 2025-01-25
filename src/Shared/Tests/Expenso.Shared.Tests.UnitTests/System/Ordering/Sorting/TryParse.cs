using Expenso.Shared.System.Types.Ordering;
using Expenso.Shared.Tests.Utils.UnitTests.Assertions;

using NUnit.Framework;

using Shouldly;

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
        result.ShouldBeFalse();
        sorting.ShouldBeNull();
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
        result.ShouldBeFalse();
        sorting.ShouldBeNull();
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
        result.ShouldBeFalse();
        sorting.ShouldBeNull();
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
        result.ShouldBeFalse();
        sorting.ShouldBeNull();
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
        result.ShouldBeFalse();
        sorting.ShouldBeNull();
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
        result.ShouldBeTrue();
        sorting.ShouldNotBeNull();
        sorting.Sorters?.Count().ShouldBe(expected: 2);
        sorting.Sorters?.ShouldContainSingle(predicate: s => s is { SortBy: "Name", SortOrder: SortOrder.Ascending });
        sorting.Sorters?.ShouldContainSingle(predicate: s => s is { SortBy: "Id", SortOrder: SortOrder.Descending });
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
        result.ShouldBeTrue();
        sorting.ShouldNotBeNull();
        sorting.Sorters?.Count().ShouldBe(expected: 2);
        sorting.Sorters?.ShouldContainSingle(predicate: s => s is { SortBy: "Name", SortOrder: SortOrder.Ascending });
        sorting.Sorters?.ShouldContainSingle(predicate: s => s is { SortBy: "Id", SortOrder: SortOrder.Descending });
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
        result.ShouldBeTrue();
        sorting.ShouldNotBeNull();
        sorting.Sorters?.Count().ShouldBe(expected: 1);
        sorting.Sorters?.ShouldContainSingle(predicate: s => s.SortBy == "Name" && s.SortOrder == SortOrder.Ascending);
    }
}