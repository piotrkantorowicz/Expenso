using Expenso.Shared.System.Types.Exceptions;
using Expenso.Shared.System.Types.Exceptions.Models;
using Expenso.TimeManagement.Core.Domain.Jobs.Model;
using Expenso.TimeManagement.Core.Domain.Jobs.Repositories.Specifications;

using FluentAssertions;

using Moq;

namespace Expenso.TimeManagement.Tests.UnitTests.Application.Jobs.Write.CancelJob.CancelJobEntryCommandHandler;

[TestFixture]
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
            .Setup(expression: x =>
                x.GetJobEntry(It.IsAny<JobEntryQuerySpecification>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(value: _jobEntry);

        // Act
        await TestCandidate.HandleAsync(command: _cancelJobEntryCommand,
            cancellationToken: It.IsAny<CancellationToken>());

        // Assert
        _jobEntry?.JobStatus.Should().Be(expected: JobEntryStatus.Cancelled);
        _jobEntryRepositoryMock.Verify(expression: x => x.AddOrUpdateAsync(_jobEntry!, It.IsAny<CancellationToken>()));
    }

    [Test]
    public async Task Should_ThrowNoFoundException_When_JobEntryNotFound()
    {
        // Arrange
        _jobEntryRepositoryMock
            .Setup(expression: x =>
                x.GetJobEntry(It.IsAny<JobEntryQuerySpecification>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(value: null);

        // Act
        Func<Task> action = async () => await TestCandidate.HandleAsync(command: _cancelJobEntryCommand,
            cancellationToken: It.IsAny<CancellationToken>());

        // Assert
        await action
            .Should()
            .ThrowAsync<NotFoundException>()
            .WithMessage(expectedWildcardPattern: $"{nameof(JobEntry)} with ID {_jobEntryId} hasn't been found.")
            .Where(exceptionExpression: x =>
                x.ResourceName == nameof(JobEntry) && x.IdentifierType == IdentifierType.PrimaryId() &&
                (Guid?)x.Identifier == _jobEntryId);
    }

    [Test]
    public async Task Should_ThrowNoFoundException_When_JobRunningStatusNotFound()
    {
        // Arrange
        _jobEntryStatusReposiotry
            .Setup(expression: x => x.GetAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>(), true))
            .ReturnsAsync(value: null);

        _jobEntryRepositoryMock
            .Setup(expression: x =>
                x.GetJobEntry(It.IsAny<JobEntryQuerySpecification>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(value: _jobEntry);

        // Act
        Func<Task> action = async () => await TestCandidate.HandleAsync(command: _cancelJobEntryCommand,
            cancellationToken: It.IsAny<CancellationToken>());

        // Assert        
        await action
            .Should()
            .ThrowAsync<NotFoundException>()
            .WithMessage(
                expectedWildcardPattern:
                $"{nameof(JobEntryStatus)} with ID {JobEntryStatus.Cancelled.Id} hasn't been found.")
            .Where(exceptionExpression: x =>
                x.ResourceName == nameof(JobEntryStatus) && x.IdentifierType == IdentifierType.PrimaryId() &&
                (Guid?)x.Identifier == JobEntryStatus.Cancelled.Id);
    }
}