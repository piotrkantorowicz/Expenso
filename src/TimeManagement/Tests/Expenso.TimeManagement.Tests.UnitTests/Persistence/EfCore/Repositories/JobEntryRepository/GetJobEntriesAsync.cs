using Expenso.Shared.System.Types.Pagination;
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
            pagination: Paging.Default, cancellationToken: default);

        // Assert
        jobEntries.Should().NotBeNull();
        jobEntries.CurrentPage.Should().Be(expected: Paging.Default.Page);
        jobEntries.TotalPages.Should().Be(expected: Paging.Default.Page);
        jobEntries.ResultsPerPage.Should().Be(expected: Paging.Default.Limit);
        jobEntries.TotalResults.Should().Be(expected: _jobEntriesIds.Count);
        jobEntries.Items.Should().HaveCount(expected: _jobEntriesIds.Count);
    }
}