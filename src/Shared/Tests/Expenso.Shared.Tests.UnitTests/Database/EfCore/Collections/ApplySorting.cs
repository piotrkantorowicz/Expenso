using Expenso.Shared.Database.EfCore.Collections;
using Expenso.Shared.Database.Ordering;

using NUnit.Framework;

using Shouldly;

namespace Expenso.Shared.Tests.UnitTests.Database.EfCore.Collections;

[TestFixture]
internal sealed class ApplySorting
{
    [Test]
    public void Should_SortBySinglePropertyAscending()
    {
        // Arrange
        IQueryable<TestEntity> data = new List<TestEntity>
        {
            new(Id: 2, Name: "B"),
            new(Id: 1, Name: "A"),
            new(Id: 3, Name: "C")
        }.AsQueryable();

        DatabaseSorting sorters = DatabaseSorting.New(sorters: new List<DatabaseSorter>
        {
            new(SortBy: "Id", SortOrder: DatabaseSortOrder.Ascending)
        });

        // Act
        List<TestEntity> sortedData = data.ApplySorting(sorting: sorters).ToList();

        // Assert
        sortedData.Select(selector: x => x.Id).ShouldBeInOrder(expectedSortDirection: SortDirection.Ascending);
    }

    [Test]
    public void Should_SortBySinglePropertyDescending()
    {
        // Arrange
        IQueryable<TestEntity> data = new List<TestEntity>
        {
            new(Id: 2, Name: "B"),
            new(Id: 1, Name: "A"),
            new(Id: 3, Name: "C")
        }.AsQueryable();

        DatabaseSorting sorters = DatabaseSorting.New(sorters: new List<DatabaseSorter>
        {
            new(SortBy: "Id", SortOrder: DatabaseSortOrder.Descending)
        });

        // Act
        List<TestEntity> sortedData = data.ApplySorting(sorting: sorters).ToList();

        // Assert
        sortedData.Select(selector: x => x.Id).ShouldBeInOrder(expectedSortDirection: SortDirection.Descending);
    }

    [Test]
    public void Should_SortByMultipleProperties()
    {
        // Arrange
        IQueryable<TestEntity> data = new List<TestEntity>
        {
            new(Id: 2, Name: "B"),
            new(Id: 1, Name: "A"),
            new(Id: 3, Name: "A")
        }.AsQueryable();

        DatabaseSorting sorters = DatabaseSorting.New(sorters: new List<DatabaseSorter>
        {
            new(SortBy: "Name", SortOrder: DatabaseSortOrder.Ascending),
            new(SortBy: "Id", SortOrder: DatabaseSortOrder.Descending)
        });

        // Act
        List<TestEntity> sortedData = data.ApplySorting(sorting: sorters).ToList();

        // Assert
        sortedData.ShouldBeEquivalentTo(expected: new List<TestEntity>
        {
            new(Id: 3, Name: "A"),
            new(Id: 1, Name: "A"),
            new(Id: 2, Name: "B")
        });
    }

    [Test]
    public void Should_HandleNullSortCriteria()
    {
        // Arrange
        List<TestEntity> data =
        [
            new(Id: 2, Name: "B"),
            new(Id: 1, Name: "A"),
            new(Id: 3, Name: "C")
        ];

        IQueryable<TestEntity> dataQueryable = data.AsQueryable();

        // Act
        List<TestEntity> sortedData = dataQueryable.ApplySorting(sorting: null).ToList();

        // Assert
        sortedData.ShouldBeEquivalentTo(expected: data);
    }

    [Test]
    public void Should_HandleEmptySortCriteria()
    {
        // Arrange
        List<TestEntity> data =
        [
            new(Id: 2, Name: "B"),
            new(Id: 1, Name: "A"),
            new(Id: 3, Name: "C")
        ];

        IQueryable<TestEntity> dataQueryable = data.AsQueryable();

        // Act
        List<TestEntity> sortedData = dataQueryable.ApplySorting(sorting: DatabaseSorting.Default).ToList();

        // Assert
        sortedData.ShouldBeEquivalentTo(expected: data);
    }

    [Test, TestCase(arguments: null, TestName = "Should_ReturnSourceForNullProperty"),
     TestCase(arg: "", TestName = "Should_ReturnSourceForEmptyProperty"),
     TestCase(arg: "InvalidProperty", TestName = "Should_ReturnSourceForInvalidProperty")]
    public void Should_ReturnSourceForInvalidProperties(string invalidProperty)
    {
        // Arrange
        List<TestEntity> data =
        [
            new(Id: 2, Name: "B"),
            new(Id: 1, Name: "A"),
            new(Id: 3, Name: "C")
        ];

        IQueryable<TestEntity> dataQueryable = data.AsQueryable();

        DatabaseSorting sorters = DatabaseSorting.New(sorters: new List<DatabaseSorter>
        {
            new(SortBy: invalidProperty, SortOrder: DatabaseSortOrder.Ascending)
        });

        // Act
        List<TestEntity> sortedData = dataQueryable.ApplySorting(sorting: sorters).ToList();

        // Assert
        sortedData.ShouldBeEquivalentTo(expected: data);
    }

    private sealed record TestEntity(int Id, string Name);
}