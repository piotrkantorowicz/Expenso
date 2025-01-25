using Expenso.Shared.System.Types.Exceptions;
using Expenso.Shared.System.Types.Exceptions.Models;
using Expenso.TimeManagement.Core.Domain.JobEntries.Model;
using Expenso.TimeManagement.Core.Domain.JobEntries.Repositories.Specifications;

using Moq;

using NUnit.Framework;

using Shouldly;

namespace Expenso.TimeManagement.Tests.UnitTests.Application.JobEntries.Write.CancelJob.CancelJobEntryCommandHandler;

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
                x.GetJobEntryAsync(It.Is<JobEntryQuerySpecification>(spec => spec.JobEntryId == _jobEntryId),
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(value: _jobEntry);

        // Act
        await TestCandidate.HandleAsync(command: _cancelJobEntryCommand,
            cancellationToken: It.IsAny<CancellationToken>());

        // Assert
        _jobEntry?.JobStatus.ShouldBe(expected: JobEntryStatus.Cancelled);
        _jobEntryRepositoryMock.Verify(expression: x => x.AddOrUpdateAsync(_jobEntry!, It.IsAny<CancellationToken>()));
    }

    [Test]
    public async Task Should_ThrowNotFoundException_When_JobEntryNotFound()
    {
        // Arrange
        _jobEntryRepositoryMock
            .Setup(expression: x =>
                x.GetJobEntryAsync(It.IsAny<JobEntryQuerySpecification>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(value: null);

        // Act
        Func<Task> action = async () => await TestCandidate.HandleAsync(command: _cancelJobEntryCommand,
            cancellationToken: It.IsAny<CancellationToken>());

        // Assert
        NotFoundException? exception = await action.ShouldThrowAsync<NotFoundException>();
        exception.Message.ShouldBe(expected: $"{nameof(JobEntry)} with ID {_jobEntryId} hasn't been found.");
        exception.ResourceName.ShouldBe(expected: nameof(JobEntry));
        exception.IdentifierType.ShouldBe(expected: IdentifierType.PrimaryId());
        ((Guid?)exception.Identifier).ShouldBe(expected: _jobEntryId);
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
                x.GetJobEntryAsync(It.IsAny<JobEntryQuerySpecification>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(value: _jobEntry);

        // Act
        Func<Task> action = async () => await TestCandidate.HandleAsync(command: _cancelJobEntryCommand,
            cancellationToken: It.IsAny<CancellationToken>());

        // Assert        
        NotFoundException? exception = await action.ShouldThrowAsync<NotFoundException>();

        exception.Message.ShouldBe(
            expected: $"{nameof(JobEntryStatus)} with ID {JobEntryStatus.Cancelled.Id} hasn't been found.");

        exception.ResourceName.ShouldBe(expected: nameof(JobEntryStatus));
        exception.IdentifierType.ShouldBe(expected: IdentifierType.PrimaryId());
        ((Guid?)exception.Identifier).ShouldBe(expected: JobEntryStatus.Cancelled.Id);
    }
}