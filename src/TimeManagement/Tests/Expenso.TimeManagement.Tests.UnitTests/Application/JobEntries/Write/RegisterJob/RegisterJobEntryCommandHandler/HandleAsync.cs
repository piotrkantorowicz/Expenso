using Expenso.Shared.System.Types.Exceptions;
using Expenso.Shared.System.Types.Exceptions.Models;
using Expenso.TimeManagement.Core.Application.JobEntries.Write.RegisterJobEntry;
using Expenso.TimeManagement.Core.Domain.JobEntries.Model;
using Expenso.TimeManagement.Core.Domain.JobEntries.Repositories.Specifications;
using Expenso.TimeManagement.Shared.DTO.RegisterJobEntry.Request;
using Expenso.TimeManagement.Shared.DTO.RegisterJobEntry.Response;

using FluentAssertions;

using Moq;

namespace Expenso.TimeManagement.Tests.UnitTests.Application.JobEntries.Write.RegisterJob.
    RegisterJobEntryCommandHandler;

[TestFixture]
internal sealed class HandleAsync : RegisterJobEntryCommandHandlerTestBase
{
    [Test]
    public async Task Should_RegisterJobEntry()
    {
        // Arrange
        _jobInstanceRepository
            .Setup(expression: x => x.GetAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>(), true))
            .ReturnsAsync(value: JobInstance.Default);

        _jobEntryStatusReposiotry
            .Setup(expression: x => x.GetAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>(), true))
            .ReturnsAsync(value: JobEntryStatus.Running);

        // Act
        RegisterJobEntryResponse response = await TestCandidate.HandleAsync(command: _registerJobEntryCommand,
            cancellationToken: It.IsAny<CancellationToken>());

        // Assert
        _jobEntryRepositoryMock.Verify(expression: x =>
            x.AddOrUpdateAsync(It.IsAny<JobEntry>(), It.IsAny<CancellationToken>()));

        response.JobEntryId.Should().Be(expected: _jobEntryId);
    }

    [Test]
    public async Task Should_ThrowConflictException_When_JobEntryAlreadyExists()
    {
        // Arrange
        _jobInstanceRepository
            .Setup(expression: x => x.GetAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>(), true))
            .ReturnsAsync(value: JobInstance.Default);

        _jobEntryStatusReposiotry
            .Setup(expression: x => x.GetAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>(), true))
            .ReturnsAsync(value: JobEntryStatus.Running);

        _jobEntryRepositoryMock
            .Setup(expression: x =>
                x.GetJobEntryAsync(It.IsAny<JobEntryQuerySpecification>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(value: new JobEntry
            {
                Id = _jobEntryId
            });

        // Act
        Func<Task> action = async () => await TestCandidate.HandleAsync(command: _registerJobEntryCommand,
            cancellationToken: It.IsAny<CancellationToken>());

        // Assert
        await action
            .Should()
            .ThrowAsync<ConflictException>()
            .WithMessage(expectedWildcardPattern: $"Job entry with id {_jobEntryId} already exists.");
    }

    [Test]
    public async Task Should_ThrowNoFoundException_When_JobInstanceNotFound()
    {
        // Arrange
        _jobInstanceRepository
            .Setup(expression: x => x.GetAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>(), true))
            .ReturnsAsync(value: null);

        // Act
        Func<Task> action = async () => await TestCandidate.HandleAsync(command: _registerJobEntryCommand,
            cancellationToken: It.IsAny<CancellationToken>());

        // Assert
        await action
            .Should()
            .ThrowAsync<NotFoundException>()
            .WithMessage(
                expectedWildcardPattern: $"{nameof(JobInstance)} with ID {JobInstance.Default.Id} hasn't been found.")
            .Where(exceptionExpression: x =>
                x.ResourceName == nameof(JobInstance) && x.IdentifierType == IdentifierType.PrimaryId() &&
                (Guid?)x.Identifier == JobInstance.Default.Id);
    }

    [Test]
    public async Task Should_ThrowNoFoundException_When_JobRunningStatusNotFound()
    {
        // Arrange
        RegisterJobEntryCommand entryCommand = new(MessageContext: MessageContextFactoryMock.Object.Current(),
            Payload: new RegisterJobEntryRequest(MaxRetries: 5, JobEntryTriggers:
            [
                new RegisterJobEntryRequestJobEntryTrigger(
                    EventType: RegisterJobEntryRequestJobEntryTriggerAllowedEventType.BudgetPermissionRequestExpired,
                    EventData: _serializer.Object.Serialize(value: _eventTrigger))
            ], Interval: null, RunAt: _clockMock.Object.UtcNow));

        _jobInstanceRepository
            .Setup(expression: x => x.GetAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>(), true))
            .ReturnsAsync(value: JobInstance.Default);

        _jobEntryStatusReposiotry
            .Setup(expression: x => x.GetAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>(), true))
            .ReturnsAsync(value: null);

        // Act
        Func<Task> action = async () =>
            await TestCandidate.HandleAsync(command: entryCommand, cancellationToken: It.IsAny<CancellationToken>());

        // Assert
        await action
            .Should()
            .ThrowAsync<NotFoundException>()
            .WithMessage(
                expectedWildcardPattern:
                $"{nameof(JobEntryStatus)} with ID {JobEntryStatus.Running.Id} hasn't been found.")
            .Where(exceptionExpression: x =>
                x.ResourceName == nameof(JobEntryStatus) && x.IdentifierType == IdentifierType.PrimaryId() &&
                (Guid?)x.Identifier == JobEntryStatus.Running.Id);
    }
}