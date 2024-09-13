using Expenso.Shared.System.Types.Exceptions;
using Expenso.TimeManagement.Core.Domain.Jobs.Model;

using FluentAssertions;

using Moq;

namespace Expenso.TimeManagement.Tests.UnitTests.Application.Jobs.Write.CancelJob.CancelJobEntryCommandHandler;

internal sealed class HandleAsync : CancelJobEntryCommandHandlerTestBase
{
    [Test]
    public async Task Should_RegisterJobEntry()
    {
        // Arrange
        _jobEntryStatusReposiotry
            .Setup(expression: x => x.GetAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>(), true))
            .ReturnsAsync(value: JobEntryStatus.Cancelled);

        _jobEntryRepositoryMock
            .Setup(expression: x => x.GetJobEntry(_jobEntryId, It.IsAny<CancellationToken>(), true))
            .ReturnsAsync(value: _jobEntry);

        // Act
        await TestCandidate.HandleAsync(command: _cancelJobEntryCommand,
            cancellationToken: It.IsAny<CancellationToken>());

        // Assert
        _jobEntry?.JobStatus.Should().Be(expected: JobEntryStatus.Cancelled);
        _jobEntryRepositoryMock.Verify(expression: x => x.AddOrUpdateAsync(_jobEntry!, It.IsAny<CancellationToken>()));
    }

    [Test]
    public void Should_ThrowNoFoundException_When_JobEntryNotFound()
    {
        // Arrange
        _jobEntryRepositoryMock
            .Setup(expression: x => x.GetJobEntry(_jobEntryId, It.IsAny<CancellationToken>(), true))
            .ReturnsAsync(value: null);

        // Act
        Func<Task> action = async () => await TestCandidate.HandleAsync(command: _cancelJobEntryCommand,
            cancellationToken: It.IsAny<CancellationToken>());

        // Assert
        action
            .Should()
            .ThrowAsync<NotFoundException>()
            .WithMessage(expectedWildcardPattern: $"Job entry with id {_jobEntryId} not found");
    }

    [Test]
    public void Should_ThrowNoFoundException_When_JobRunningStatusNotFound()
    {
        // Arrange
        _jobEntryStatusReposiotry
            .Setup(expression: x => x.GetAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>(), true))
            .ReturnsAsync(value: null);

        _jobEntryRepositoryMock
            .Setup(expression: x => x.GetJobEntry(_jobEntryId, It.IsAny<CancellationToken>(), true))
            .ReturnsAsync(value: _jobEntry);

        // Act
        Func<Task> action = async () => await TestCandidate.HandleAsync(command: _cancelJobEntryCommand,
            cancellationToken: It.IsAny<CancellationToken>());

        // Assert        
        action
            .Should()
            .ThrowAsync<NotFoundException>()
            .WithMessage(expectedWildcardPattern: $"Job status with id {JobEntryStatus.Cancelled.Id} not found");
    }
}