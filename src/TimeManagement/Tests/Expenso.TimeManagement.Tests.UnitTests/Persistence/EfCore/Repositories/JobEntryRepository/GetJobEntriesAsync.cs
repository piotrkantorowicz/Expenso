using Expenso.Shared.Database.Ordering;
using Expenso.Shared.Database.Paging;
using Expenso.Shared.System.Types.Paging;
using Expenso.TimeManagement.Core.Domain.JobEntries.Model;
using Expenso.TimeManagement.Core.Domain.JobEntries.Repositories.Specifications;

using NUnit.Framework;

using Shouldly;

namespace Expenso.TimeManagement.Tests.UnitTests.Persistence.EfCore.Repositories.JobEntryRepository;

[TestFixture]
internal sealed class GetJobEntriesAsync : JobEntryRepositoryTestBase
{
    [Test]
    public async Task Should_ReturnJobEntries_When_JobEntryExists()
    {
        // Arrange
        JobEntryQuerySpecification querySpecification = new(UseTracking: false);

        // Act 
        IPagedList<JobEntry> jobEntries = await TestCandidate.GetJobEntriesAsync(querySpecification: querySpecification,
            pagination: DatabasePagination.Default, sorters: DatabaseSorting.Default, cancellationToken: default);

        // Assert
        jobEntries.ShouldNotBeNull();
        jobEntries.CurrentPage.ShouldBe(expected: DatabasePagination.Default.Page);

        jobEntries.TotalPages.ShouldBe(
            expected: (int)Math.Ceiling(a: _jobEntriesIds.Count / (double)DatabasePagination.Default.Limit));

        jobEntries.ResultsPerPage.ShouldBe(expected: DatabasePagination.Default.Limit);
        jobEntries.TotalResults.ShouldBe(expected: _jobEntriesIds.Count);
        jobEntries.Items.Count.ShouldBe(expected: _jobEntriesIds.Count);
    }

    [Test]
    public async Task Should_ReturnCorrectPage_When_CustomPaginationProvided()
    {
        // Arrange
        JobEntryQuerySpecification querySpecification = new(UseTracking: false);
        DatabasePagination pagination = DatabasePagination.New(page: 2, limit: 5);

        // Act 
        IPagedList<JobEntry> jobEntries = await TestCandidate.GetJobEntriesAsync(querySpecification: querySpecification,
            pagination: pagination, sorters: DatabaseSorting.Default, cancellationToken: default);

        // Assert
        jobEntries.ShouldNotBeNull();
        jobEntries.CurrentPage.ShouldBe(expected: 2);
        jobEntries.ResultsPerPage.ShouldBe(expected: 5);
        jobEntries.Items.Count.ShouldBeLessThanOrEqualTo(expected: 5);
    }
}