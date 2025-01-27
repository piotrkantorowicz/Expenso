using Expenso.Shared.Database.Ordering;
using Expenso.Shared.Database.Paging;
using Expenso.Shared.System.Types.Paging;
using Expenso.TimeManagement.Core.Application.JobEntries.Read.GetJobEntries.DTO.Response;
using Expenso.TimeManagement.Core.Domain.JobEntries.Model;
using Expenso.TimeManagement.Core.Domain.JobEntries.Repositories.Specifications;

using Moq;

using NUnit.Framework;

using Shouldly;

namespace Expenso.TimeManagement.Tests.UnitTests.Application.JobEntries.Read.GetJobEntries.GetJobEntriesQueryHandler;

[TestFixture]
internal sealed class HandleAsync : GetJobEntriesQueryHandlerTestBase
{
    [Test]
    public async Task Should_ReturnJobEntriesResponse_When_JobEntriesExist()
    {
        // Arrange
        _jobEntryRepositoryMock
            .Setup(expression: x => x.GetJobEntriesAsync(It.IsAny<JobEntryQuerySpecification>(),
                DatabasePagination.Default, It.IsAny<DatabaseSorting>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(value: PagedList<JobEntry>.Create(items: _jobEntries, currentPage: 1, resultsPerPage: 10,
                totalPages: 1, totalResults: 2));

        // Act
        IPagedList<GetJobEntriesResponse>? jobEntriesResponse =
            await TestCandidate.HandleAsync(query: _getJobEntriesQuery,
                cancellationToken: It.IsAny<CancellationToken>());

        // Assert
        jobEntriesResponse?.ShouldNotBeNull();
        jobEntriesResponse?.CurrentPage.ShouldBe(expected: DatabasePagination.Default.Page);

        jobEntriesResponse?.TotalPages.ShouldBe(
            expected: (int)Math.Ceiling(a: _jobEntries.Count / (double)DatabasePagination.Default.Limit));

        jobEntriesResponse?.ResultsPerPage.ShouldBe(expected: 10);
        jobEntriesResponse?.TotalResults.ShouldBe(expected: _jobEntries.Count);
        jobEntriesResponse?.Items.Count.ShouldBe(expected: _jobEntries.Count);
    }

    [Test]
    public async Task Should_ReturnEmptyPagedList_When_JobEntriesHaveNotExists()
    {
        // Arrange
        _jobEntryRepositoryMock
            .Setup(expression: x => x.GetJobEntriesAsync(It.IsAny<JobEntryQuerySpecification>(),
                DatabasePagination.Default, It.IsAny<DatabaseSorting>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(value: PagedList<JobEntry>.AsEmpty);

        // Act
        IPagedList<GetJobEntriesResponse>? jobEntriesResponse = await TestCandidate.HandleAsync(
            query: _getJobEntriesQuery,
                cancellationToken: It.IsAny<CancellationToken>());

        // Assert
        jobEntriesResponse?.ShouldNotBeNull();
        jobEntriesResponse?.Items.Count.ShouldBe(expected: 0);
        jobEntriesResponse?.CurrentPage.ShouldBe(expected: DatabasePagination.Default.Page);
        jobEntriesResponse?.TotalPages.ShouldBe(expected: DatabasePagination.Default.Page);
        jobEntriesResponse?.ResultsPerPage.ShouldBe(expected: DatabasePagination.Default.Limit);
    }
}