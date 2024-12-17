using Expenso.Shared.System.Types.Exceptions;
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
            .Setup(expression: x =>
                x.GetJobEntriesAsync(It.IsAny<JobEntryQuerySpecification>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(value: _jobEntries);

        // Act
        IReadOnlyCollection<GetJobEntriesResponse>? jobEntriesResponse =
            await TestCandidate.HandleAsync(query: _getJobEntriesQuery,
                cancellationToken: It.IsAny<CancellationToken>());

        // Assert
        jobEntriesResponse.Should().NotBeNull();
        jobEntriesResponse?.Should().HaveCount(expected: _jobEntries.Count);
    }

    [Test]
    public void Should_ThrowNotFoundException_When_JobEntriesDoNotExist()
    {
        // Arrange
        _jobEntryRepositoryMock
            .Setup(expression: x =>
                x.GetJobEntriesAsync(It.IsAny<JobEntryQuerySpecification>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(value: new List<JobEntry>());

        // Act
        Func<Task> act = async () =>
            await TestCandidate.HandleAsync(query: _getJobEntriesQuery,
                cancellationToken: It.IsAny<CancellationToken>());

        // Assert
        act.Should().ThrowAsync<NotFoundException>();
    }
}