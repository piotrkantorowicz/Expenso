using Expenso.Shared.System.Types.Exceptions;
using Expenso.TimeManagement.Core.Domain.JobEntries.Repositories.Specifications;
using Expenso.TimeManagement.Shared.DTO.GetJobEntry.Response;

using FluentAssertions;

using Moq;

namespace Expenso.TimeManagement.Tests.UnitTests.Application.JobEntries.Read.GetJobEntry.GetJobEntryQueryHandler;

[TestFixture]
internal sealed class HandleAsync : GetJobEntryQueryHandlerTestBase
{
    [Test]
    public async Task Should_ReturnJobEntryResponse_When_JobEntryExists()
    {
        // Arrange
        _jobEntryRepositoryMock
            .Setup(expression: x =>
                x.GetJobEntryAsync(It.IsAny<JobEntryQuerySpecification>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(value: _jobEntry);

        // Act
        GetJobEntryResponse? jobEntryResponse = await TestCandidate.HandleAsync(query: _getJobEntryQuery,
            cancellationToken: It.IsAny<CancellationToken>());

        // Assert
        jobEntryResponse.Should().NotBeNull();
        jobEntryResponse?.Id.Should().Be(expected: _jobEntryId);
    }

    [Test]
    public void Should_ThrowNotFoundException_When_JobEntryDoesNotExist()
    {
        // Arrange
        _jobEntryRepositoryMock
            .Setup(expression: x =>
                x.GetJobEntryAsync(It.IsAny<JobEntryQuerySpecification>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(value: null);

        // Act
        Func<Task> act = async () =>
            await TestCandidate.HandleAsync(query: _getJobEntryQuery, cancellationToken: It.IsAny<CancellationToken>());

        // Assert
        act.Should().ThrowAsync<NotFoundException>();
    }
}