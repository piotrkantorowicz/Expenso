using Expenso.Shared.Database.Paging;
using Expenso.Shared.System.Types.Ordering;
using Expenso.Shared.System.Types.Paging;
using Expenso.TimeManagement.Core.Domain.JobEntries.Model;
using Expenso.TimeManagement.Core.Domain.JobEntries.Repositories.Specifications;

using FluentAssertions;

using NUnit.Framework;

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
            pagination: DatabasePagination.Default, sorters: Sorting.Default, cancellationToken: default);

        // Assert
        jobEntries.Should().NotBeNull();
        jobEntries.CurrentPage.Should().Be(expected: DatabasePagination.Default.Page);

        jobEntries
            .TotalPages.Should()
            .Be(expected: (int)Math.Ceiling(a: _jobEntriesIds.Count / (double)DatabasePagination.Default.Limit));

        jobEntries.ResultsPerPage.Should().Be(expected: DatabasePagination.Default.Limit);
        jobEntries.TotalResults.Should().Be(expected: _jobEntriesIds.Count);
        jobEntries.Items.Should().HaveCount(expected: _jobEntriesIds.Count);
    }

    [Test]
    public async Task Should_ReturnCorrectPage_When_CustomPaginationProvided()
    {
        // Arrange
        JobEntryQuerySpecification querySpecification = new(UseTracking: false);
        DatabasePagination pagination = DatabasePagination.New(page: 2, limit: 5);

        // Act 
        IPagedList<JobEntry> jobEntries = await TestCandidate.GetJobEntriesAsync(querySpecification: querySpecification,
            pagination: pagination, sorters: Sorting.Default, cancellationToken: default);

        // Assert
        jobEntries.Should().NotBeNull();
        jobEntries.CurrentPage.Should().Be(expected: 2);
        jobEntries.ResultsPerPage.Should().Be(expected: 5);
        jobEntries.Items.Should().HaveCountLessOrEqualTo(expected: 5);
    }
}