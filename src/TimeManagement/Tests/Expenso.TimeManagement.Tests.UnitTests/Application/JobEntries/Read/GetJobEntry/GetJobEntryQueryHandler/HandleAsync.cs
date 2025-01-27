using Expenso.Shared.System.Types.Exceptions;
using Expenso.TimeManagement.Core.Domain.JobEntries.Repositories.Specifications;
using Expenso.TimeManagement.Shared.DTO.GetJobEntry.Response;

using Moq;

using NUnit.Framework;

using Shouldly;

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
        jobEntryResponse.ShouldNotBeNull();
        jobEntryResponse?.Id.ShouldBe(expected: _jobEntryId);
    }

    [Test]
    public async Task Should_ThrowNotFoundException_When_JobEntryDoesNotExist()
    {
        // Arrange
        _jobEntryRepositoryMock
            .Setup(expression: x =>
                x.GetJobEntryAsync(It.IsAny<JobEntryQuerySpecification>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(value: null);

        // Act
        Func<Task> action = async () =>
            await TestCandidate.HandleAsync(query: _getJobEntryQuery, cancellationToken: It.IsAny<CancellationToken>());

        // Assert
        await action.ShouldThrowAsync<NotFoundException>();
    }
}