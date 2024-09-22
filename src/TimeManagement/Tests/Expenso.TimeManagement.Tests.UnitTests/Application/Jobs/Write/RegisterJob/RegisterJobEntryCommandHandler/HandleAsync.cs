using Expenso.BudgetSharing.Proxy.DTO.MessageBus.BudgetPermissionRequests;
using Expenso.Shared.System.Types.Exceptions;
using Expenso.TimeManagement.Core.Application.Jobs.Write.RegisterJob;
using Expenso.TimeManagement.Core.Domain.Jobs.Model;
using Expenso.TimeManagement.Proxy.DTO.Request;
using Expenso.TimeManagement.Proxy.DTO.Response;

using FluentAssertions;

using Moq;

namespace Expenso.TimeManagement.Tests.UnitTests.Application.Jobs.Write.RegisterJob.RegisterJobEntryCommandHandler;

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
        await TestCandidate.HandleAsync(entryCommand: _registerJobEntryCommand,
            cancellationToken: It.IsAny<CancellationToken>());

        // Assert
        _jobEntryRepositoryMock.Verify(expression: x =>
            x.AddOrUpdateAsync(It.IsAny<JobEntry>(), It.IsAny<CancellationToken>()));
    }

    [Test]
    public async Task Should_RegisterJobEntry_And_CorrectlySetInterval()
    {
        // Arrange
        _jobInstanceRepository
            .Setup(expression: x => x.GetAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>(), true))
            .ReturnsAsync(value: JobInstance.Default);

        _jobEntryStatusReposiotry
            .Setup(expression: x => x.GetAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>(), true))
            .ReturnsAsync(value: JobEntryStatus.Running);

        RegisterJobEntryCommand entryCommand = _registerJobEntryCommand with
        {
            RegisterJobEntryRequest = _registerJobEntryCommand.RegisterJobEntryRequest! with
            {
                Interval = new RegisterJobEntryRequest_JobEntryPeriodInterval(DayOfWeek: 5, Month: 6, DayofMonth: 10,
                    Hour: 12, Minute: 30, Second: 30, UseSeconds: true)
            }
        };

        // Act
        RegisterJobEntryResponse? result = await TestCandidate.HandleAsync(entryCommand: entryCommand,
            cancellationToken: It.IsAny<CancellationToken>());

        // Assert
        _jobEntryRepositoryMock.Verify(expression: x =>
            x.AddOrUpdateAsync(It.IsAny<JobEntry>(), It.IsAny<CancellationToken>()));

        result?.CronExpression.Should().Be(expected: "30 30 12 10 6 5");
    }

    [Test]
    public void Should_ThrowNoFoundException_When_JobInstanceNotFound()
    {
        // Arrange
        _jobInstanceRepository
            .Setup(expression: x => x.GetAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>(), true))
            .ReturnsAsync(value: null);

        // Act
        Func<Task> action = async () => await TestCandidate.HandleAsync(entryCommand: _registerJobEntryCommand,
            cancellationToken: It.IsAny<CancellationToken>());

        // Assert
        action
            .Should()
            .ThrowAsync<NotFoundException>()
            .WithMessage(expectedWildcardPattern: $"Job instance with id {JobInstance.Default.Id} not found.");
    }

    [Test]
    public void Should_ThrowNoFoundException_When_JobRunningStatusNotFound()
    {
        // Arrange
        RegisterJobEntryCommand entryCommand = new(MessageContext: MessageContextFactoryMock.Object.Current(),
            RegisterJobEntryRequest: new RegisterJobEntryRequest(MaxRetries: 5, JobEntryTriggers:
            [
                new RegisterJobEntryRequest_JobEntryTrigger(
                    EventType: typeof(BudgetPermissionRequestExpiredIntegrationEvent).AssemblyQualifiedName,
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
            await TestCandidate.HandleAsync(entryCommand: entryCommand,
                cancellationToken: It.IsAny<CancellationToken>());

        // Assert
        action
            .Should()
            .ThrowAsync<NotFoundException>()
            .WithMessage(expectedWildcardPattern: $"Job status with id {JobEntryStatus.Running.Id} not found.");
    }

    [Test]
    public void Should_ThrowNoFoundException_When_RegisterJobEntryRequestIsNull()
    {
        // Arrange
        RegisterJobEntryCommand entryCommand = _registerJobEntryCommand with
        {
            RegisterJobEntryRequest = null
        };

        _jobInstanceRepository
            .Setup(expression: x => x.GetAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>(), true))
            .ReturnsAsync(value: JobInstance.Default);

        _jobEntryStatusReposiotry
            .Setup(expression: x => x.GetAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>(), true))
            .ReturnsAsync(value: JobEntryStatus.Running);

        // Act
        Func<Task> action = async () =>
            await TestCandidate.HandleAsync(entryCommand: entryCommand,
                cancellationToken: It.IsAny<CancellationToken>());

        // Assert
        action
            .Should()
            .ThrowAsync<NotFoundException>()
            .WithMessage(expectedWildcardPattern: "Unable to create job entry from request.");
    }
}