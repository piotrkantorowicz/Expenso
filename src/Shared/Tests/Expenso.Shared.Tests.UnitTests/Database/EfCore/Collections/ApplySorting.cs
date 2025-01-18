using Expenso.Shared.Database.EfCore.Collections;
using Expenso.Shared.System.Types.Ordering;

using FluentAssertions;

using NUnit.Framework;

namespace Expenso.Shared.Tests.UnitTests.Database.EfCore.Collections;

[TestFixture]
internal sealed class ApplySorting
{
    [Test]
    public void Should_SortBySinglePropertyAscending()
    {
        // Arrange
        IQueryable<TestEntity>? data = new List<TestEntity>
        {
            new(Id: 2, Name: "B"),
            new(Id: 1, Name: "A"),
            new(Id: 3, Name: "C")
        }.AsQueryable();

        Sorting? sorters = Sorting.New(sorters: new List<ISorter>
        {
            new Sorter(SortBy: "Id", SortOrder: SortOrder.Ascending)
        });

        // Act
        List<TestEntity>? sortedData = data.ApplySorting(sorting: sorters).ToList();

        // Assert
        sortedData.Should().BeInAscendingOrder(propertyExpression: e => e.Id);
    }

    [Test]
    public void Should_SortBySinglePropertyDescending()
    {
        // Arrange
        IQueryable<TestEntity>? data = new List<TestEntity>
        {
            new(Id: 2, Name: "B"),
            new(Id: 1, Name: "A"),
            new(Id: 3, Name: "C")
        }.AsQueryable();

        Sorting? sorters = Sorting.New(sorters: new List<ISorter>
        {
            new Sorter(SortBy: "Id", SortOrder: SortOrder.Descending)
        });

        // Act
        List<TestEntity>? sortedData = data.ApplySorting(sorting: sorters).ToList();

        // Assert
        sortedData.Should().BeInDescendingOrder(propertyExpression: e => e.Id);
    }

    [Test]
    public void Should_SortByMultipleProperties()
    {
        // Arrange
        IQueryable<TestEntity>? data = new List<TestEntity>
        {
            new(Id: 2, Name: "B"),
            new(Id: 1, Name: "A"),
            new(Id: 3, Name: "A")
        }.AsQueryable();

        Sorting? sorters = Sorting.New(sorters: new List<ISorter>
        {
            new Sorter(SortBy: "Name", SortOrder: SortOrder.Ascending),
            new Sorter(SortBy: "Id", SortOrder: SortOrder.Descending)
        });

        // Act
        List<TestEntity>? sortedData = data.ApplySorting(sorting: sorters).ToList();

        // Assert
        sortedData
            .Should()
            .BeEquivalentTo(expectation: new List<TestEntity>
            {
                new(Id: 3, Name: "A"),
                new(Id: 1, Name: "A"),
                new(Id: 2, Name: "B")
            }, config: options => options.WithStrictOrdering());
    }

    [Test]
    public void Should_HandleNullSortCriteria()
    {
        // Arrange
        IQueryable<TestEntity>? data = new List<TestEntity>
        {
            new(Id: 2, Name: "B"),
            new(Id: 1, Name: "A"),
            new(Id: 3, Name: "C")
        }.AsQueryable();

        // Act
        List<TestEntity>? sortedData = data.ApplySorting(sorting: null).ToList();

        // Assert
        sortedData.Should().BeEquivalentTo(expectation: data);
    }

    [Test]
    public void Should_HandleEmptySortCriteria()
    {
        // Arrange
        IQueryable<TestEntity>? data = new List<TestEntity>
        {
            new(Id: 2, Name: "B"),
            new(Id: 1, Name: "A"),
            new(Id: 3, Name: "C")
        }.AsQueryable();

        // Act
        List<TestEntity>? sortedData = data.ApplySorting(sorting: Sorting.Default).ToList();

        // Assert
        sortedData.Should().BeEquivalentTo(expectation: data);
    }

    [Test, TestCase(arguments: null, TestName = "Should_ReturnSourceForNullProperty"),
     TestCase(arg: "", TestName = "Should_ReturnSourceForEmptyProperty"),
     TestCase(arg: "InvalidProperty", TestName = "Should_ReturnSourceForInvalidProperty")]
    public void Should_ReturnSourceForInvalidProperties(string invalidProperty)
    {
        // Arrange
        IQueryable<TestEntity>? data = new List<TestEntity>
        {
            new(Id: 2, Name: "B"),
            new(Id: 1, Name: "A"),
            new(Id: 3, Name: "C")
        }.AsQueryable();

        Sorting? sorters = Sorting.New(sorters: new List<ISorter>
        {
            new Sorter(SortBy: invalidProperty, SortOrder: SortOrder.Ascending)
        });

        // Act
        List<TestEntity>? sortedData = data.ApplySorting(sorting: sorters).ToList();

        // Assert
        sortedData.Should().BeEquivalentTo(expectation: data);
    }

    private sealed record TestEntity(int Id, string Name);
}