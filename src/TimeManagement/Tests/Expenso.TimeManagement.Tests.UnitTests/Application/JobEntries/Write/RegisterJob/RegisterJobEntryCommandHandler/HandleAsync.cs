using Expenso.Shared.System.Types.Exceptions;
using Expenso.Shared.System.Types.Exceptions.Models;
using Expenso.TimeManagement.Core.Application.JobEntries.Write.RegisterJobEntry;
using Expenso.TimeManagement.Core.Domain.JobEntries.Model;
using Expenso.TimeManagement.Core.Domain.JobEntries.Repositories.Specifications;
using Expenso.TimeManagement.Shared.DTO.RegisterJobEntry.Request;
using Expenso.TimeManagement.Shared.DTO.RegisterJobEntry.Response;

using Moq;

using NUnit.Framework;

using Shouldly;

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

        response.JobEntryId.ShouldBe(expected: _jobEntryId);
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

        JobEntryQuerySpecification jobEntryQuerySpecification = new(JobEntryId: _jobEntryId, UseTracking: false);

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
        ConflictException? exception = await action.ShouldThrowAsync<ConflictException>();
        exception.Message.ShouldBe(expected: $"JobEntry with query {jobEntryQuerySpecification} already exists.");
        exception.ResourceName.ShouldBe(expected: nameof(JobEntry));
        exception.IdentifierType.ShouldBe(expected: IdentifierType.Query());
        ((JobEntryQuerySpecification?)exception.Identifier).ShouldBeEquivalentTo(expected: jobEntryQuerySpecification);
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
        NotFoundException? exception = await action.ShouldThrowAsync<NotFoundException>();

        exception.Message.ShouldBe(
            expected: $"{nameof(JobInstance)} with ID {JobInstance.Default.Id} hasn't been found.");

        exception.ResourceName.ShouldBe(expected: nameof(JobInstance));
        exception.IdentifierType.ShouldBe(expected: IdentifierType.PrimaryId());
        ((Guid?)exception.Identifier).ShouldBe(expected: JobInstance.Default.Id);
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
        NotFoundException? exception = await action.ShouldThrowAsync<NotFoundException>();

        exception.Message.ShouldBe(
            expected: $"{nameof(JobEntryStatus)} with ID {JobEntryStatus.Running.Id} hasn't been found.");

        exception.ResourceName.ShouldBe(expected: nameof(JobEntryStatus));
        exception.IdentifierType.ShouldBe(expected: IdentifierType.PrimaryId());
        ((Guid?)exception.Identifier).ShouldBe(expected: JobEntryStatus.Running.Id);
    }
}