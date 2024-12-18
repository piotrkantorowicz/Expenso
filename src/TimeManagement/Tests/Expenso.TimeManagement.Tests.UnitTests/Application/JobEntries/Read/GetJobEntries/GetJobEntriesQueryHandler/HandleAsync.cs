using Expenso.Shared.System.Types.Exceptions;
using Expenso.Shared.System.Types.Pagination;
using Expenso.TimeManagement.Core.Application.JobEntries.Read.GetJobEntries.DTO.Response;
using Expenso.TimeManagement.Core.Domain.JobEntries.Model;
using Expenso.TimeManagement.Core.Domain.JobEntries.Repositories.Specifications;

using FluentAssertions;

using Moq;

using NUnit.Framework;

namespace Expenso.TimeManagement.Tests.UnitTests.Application.JobEntries.Read.GetJobEntries.GetJobEntriesQueryHandler;

[TestFixture]
internal sealed class HandleAsync : GetJobEntriesQueryHandlerTestBase
{
    [Test]
    public async Task Should_ReturnJobEntriesResponse_When_JobEntriesExist()
    {
        // Arrange
        _jobEntryRepositoryMock
            .Setup(expression: x => x.GetJobEntriesAsync(It.IsAny<JobEntryQuerySpecification>(), Paging.Default,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(value: PagedList<JobEntry>.Create(items: _jobEntries, currentPage: 1, resultsPerPage: 10,
                totalPages: 1, totalResults: 2));

        // Act
        IPagedList<GetJobEntriesResponse>? jobEntriesResponse =
            await TestCandidate.HandleAsync(query: _getJobEntriesQuery,
                cancellationToken: It.IsAny<CancellationToken>());

        // Assert
        jobEntriesResponse?.Should().NotBeNull();
        jobEntriesResponse?.CurrentPage.Should().Be(expected: Paging.Default.Page);
        jobEntriesResponse?.TotalPages.Should().Be(expected: Paging.Default.Page);
        jobEntriesResponse?.ResultsPerPage.Should().Be(expected: 10);
        jobEntriesResponse?.TotalResults.Should().Be(expected: _jobEntries.Count);
        jobEntriesResponse?.Items.Should().HaveCount(expected: _jobEntries.Count);
    }

    [Test]
    public void Should_ThrowNotFoundException_When_JobEntriesDoNotExist()
    {
        // Arrange
        _jobEntryRepositoryMock
            .Setup(expression: x => x.GetJobEntriesAsync(It.IsAny<JobEntryQuerySpecification>(), Paging.Default,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(value: PagedList<JobEntry>.AsEmpty);

        // Act
        Func<Task> act = async () =>
            await TestCandidate.HandleAsync(query: _getJobEntriesQuery,
                cancellationToken: It.IsAny<CancellationToken>());

        // Assert
        act.Should().ThrowAsync<NotFoundException>();
    }
}