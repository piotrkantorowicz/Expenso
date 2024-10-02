using Expenso.BudgetSharing.Proxy.DTO.MessageBus.BudgetPermissionRequests;
using Expenso.Shared.Integration.Events;
using Expenso.Shared.System.Logging;
using Expenso.Shared.System.Serialization.Default;
using Expenso.TimeManagement.Core.Domain.Jobs.Model;

using FluentAssertions;

using Moq;

using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Expenso.TimeManagement.Tests.UnitTests.Application.Jobs.Shared.BackgroundJobs.JobExecutions;

internal sealed class Execute : JobExecutionTestBase
{
    private readonly TimeSpan _interval = TimeSpan.FromSeconds(value: 10);
    private readonly Guid _jobInstanceId = JobInstance.Default.Id;

    [Test]
    public async Task Should_LogInformation_When_JobInstanceHasNotBeenExists()
    {
        // Arrange
        _jobInstanceRepositoryMock
            .Setup(expression: x => x.GetAsync(_jobInstanceId, It.IsAny<CancellationToken>(), true))
            .ReturnsAsync(value: null);

        // Act
        await TestCandidate.Execute(jobInstanceId: _jobInstanceId, interval: _interval,
            stoppingToken: CancellationToken.None);

        // Assert
        _loggerMock.Verify(
            expression: x => x.LogWarning(LoggingUtils.BackgroundJobWarning,
                "Job instance with id: {JobInstanceId} hasn't been found", null, null, _jobInstanceId),
            times: Times.Once);
    }

    [Test]
    public async Task Should_LogInformation_When_NoActiveJobEntriesFound()
    {
        // Arrange
        _jobInstanceRepositoryMock
            .Setup(expression: x => x.GetAsync(_jobInstanceId, It.IsAny<CancellationToken>(), true))
            .ReturnsAsync(value: JobInstance.Default);

        _jobEntryRepositoryMock
            .Setup(expression: x => x.GetActiveJobEntries(It.IsAny<Guid>(), It.IsAny<CancellationToken>(), true))
            .ReturnsAsync(value: new List<JobEntry>());

        // Act
        await TestCandidate.Execute(jobInstanceId: _jobInstanceId, interval: _interval,
            stoppingToken: CancellationToken.None);

        // Assert
        _loggerMock.Verify(
            expression: x => x.LogInfo(LoggingUtils.BackgroundJobGeneralInformation,
                "No active job entries found. JobInstanceId: {JobInstanceId}", null, _jobInstanceId),
            times: Times.Once);
    }

    [Test]
    public async Task Should_LogWarning_When_NoJobEntryStatusesFound()
    {
        // Arrange
        _jobInstanceRepositoryMock
            .Setup(expression: x => x.GetAsync(_jobInstanceId, It.IsAny<CancellationToken>(), true))
            .ReturnsAsync(value: JobInstance.Default);

        _jobEntryRepositoryMock
            .Setup(expression: x => x.GetActiveJobEntries(It.IsAny<Guid>(), It.IsAny<CancellationToken>(), true))
            .ReturnsAsync(value: new List<JobEntry>
            {
                new()
            });

        _jobEntryStatusRepositoryMock
            .Setup(expression: x => x.GetAsync(It.IsAny<CancellationToken>(), true))
            .ReturnsAsync(value: new List<JobEntryStatus>());

        // Act
        await TestCandidate.Execute(jobInstanceId: _jobInstanceId, interval: _interval,
            stoppingToken: CancellationToken.None);

        // Assert
        _loggerMock.Verify(
            expression: x => x.LogWarning(LoggingUtils.BackgroundJobWarning, "No job entry statuses found", null, null),
            times: Times.Once);
    }

    [Test]
    public async Task Should_LogWarning_When_JobEntryHasNoTriggers()
    {
        // Arrange
        JobEntry jobEntry = new()
        {
            Id = Guid.NewGuid(),
            Triggers = new List<JobEntryTrigger>()
        };

        _jobInstanceRepositoryMock
            .Setup(expression: x => x.GetAsync(_jobInstanceId, It.IsAny<CancellationToken>(), true))
            .ReturnsAsync(value: JobInstance.Default);

        _jobEntryRepositoryMock
            .Setup(expression: x => x.GetActiveJobEntries(It.IsAny<Guid>(), It.IsAny<CancellationToken>(), true))
            .ReturnsAsync(value: new List<JobEntry>
            {
                jobEntry
            });

        // Act
        await TestCandidate.Execute(jobInstanceId: _jobInstanceId, interval: _interval,
            stoppingToken: CancellationToken.None);

        // Assert
        _jobEntryRepositoryMock.Verify(
            expression: x =>
                x.AddOrUpdateAsync(It.Is<JobEntry>(j => j.Id == jobEntry.Id), It.IsAny<CancellationToken>()),
            times: Times.Once);

        _loggerMock.Verify(
            expression: x => x.LogWarning(LoggingUtils.BackgroundJobWarning,
                "Skipping job entry with id {JobEntryId} because it has no triggers. JobInstanceId: {JobInstanceId}",
                null, null, jobEntry.Id, _jobInstanceId), times: Times.Once);
    }

    [Test]
    public async Task Should_SetJobAsCompleted_When_CronExpressionIsEmptyAndJobHasBeenRanBefore()
    {
        // Arrange
        DateTimeOffset utcClock = DateTimeOffset.UtcNow;

        JobEntry jobEntry = new()
        {
            Id = Guid.NewGuid(),
            LastRun = utcClock.AddMinutes(minutes: -15),
            Triggers = new List<JobEntryTrigger>
            {
                new()
            }
        };

        _clockMock.Setup(expression: x => x.UtcNow).Returns(value: DateTimeOffset.UtcNow);

        _jobInstanceRepositoryMock
            .Setup(expression: x => x.GetAsync(_jobInstanceId, It.IsAny<CancellationToken>(), true))
            .ReturnsAsync(value: JobInstance.Default);

        _jobEntryRepositoryMock
            .Setup(expression: x => x.GetActiveJobEntries(It.IsAny<Guid>(), It.IsAny<CancellationToken>(), true))
            .ReturnsAsync(value: new List<JobEntry>
            {
                jobEntry
            });

        // Act
        await TestCandidate.Execute(jobInstanceId: _jobInstanceId, interval: _interval,
            stoppingToken: CancellationToken.None);

        // Assert
        _jobEntryRepositoryMock.Verify(
            expression: x =>
                x.AddOrUpdateAsync(It.Is<JobEntry>(j => j.Id == jobEntry.Id), It.IsAny<CancellationToken>()),
            times: Times.Once);

        jobEntry.IsCompleted.Should().BeTrue();

        _loggerMock.Verify(
            expression: x => x.LogWarning(LoggingUtils.BackgroundJobWarning,
                "Skipping job entry with Id {JobEntryId} because the job is ended. JobInstanceId: {JobInstanceId}",
                null, null, jobEntry.Id, _jobInstanceId), times: Times.Once);
    }

    [Test]
    public async Task Should_SetJobAsCompleted_When_PeriodicJobWontRun()
    {
        // Arrange
        DateTimeOffset utcClock = DateTimeOffset.UtcNow;

        JobEntry jobEntry = new()
        {
            Id = Guid.NewGuid(),
            CronExpression = $"* * * * {(int)utcClock.DayOfWeek}",
            Triggers = new List<JobEntryTrigger>
            {
                new()
            }
        };

        _clockMock
            .Setup(expression: x => x.UtcNow)
            .Returns(value: new DateTime(year: utcClock.Year, month: utcClock.Month, day: utcClock.Day,
                hour: utcClock.Hour, minute: utcClock.Minute, second: 30, kind: DateTimeKind.Utc));

        _jobInstanceRepositoryMock
            .Setup(expression: x => x.GetAsync(_jobInstanceId, It.IsAny<CancellationToken>(), true))
            .ReturnsAsync(value: JobInstance.Default);

        _jobEntryRepositoryMock
            .Setup(expression: x => x.GetActiveJobEntries(It.IsAny<Guid>(), It.IsAny<CancellationToken>(), true))
            .ReturnsAsync(value: new List<JobEntry>
            {
                jobEntry
            });

        // Act
        await TestCandidate.Execute(jobInstanceId: _jobInstanceId, interval: _interval,
            stoppingToken: CancellationToken.None);

        // Assert
        _loggerMock.Verify(
            expression: x => x.LogDebug(LoggingUtils.BackgroundJobGeneralInformation,
                "Skipping job entry with Id {JobEntryId} because it's out of the actual run. JobInstanceId: {JobInstanceId}",
                null, jobEntry.Id, _jobInstanceId), times: Times.Once);
    }

    [Test]
    public async Task Should_LogWarning_When_NoTriggers()
    {
        // Arrange
        JobEntry jobEntry = new()
        {
            Id = Guid.NewGuid(),
            Triggers = new List<JobEntryTrigger>
            {
                new()
            }
        };

        _clockMock.Setup(expression: x => x.UtcNow).Returns(value: DateTimeOffset.UtcNow);

        _jobInstanceRepositoryMock
            .Setup(expression: x => x.GetAsync(_jobInstanceId, It.IsAny<CancellationToken>(), true))
            .ReturnsAsync(value: JobInstance.Default);

        _jobEntryRepositoryMock
            .Setup(expression: x => x.GetActiveJobEntries(It.IsAny<Guid>(), It.IsAny<CancellationToken>(), true))
            .ReturnsAsync(value: new List<JobEntry>
            {
                jobEntry
            });

        // Act
        await TestCandidate.Execute(jobInstanceId: _jobInstanceId, interval: _interval,
            stoppingToken: CancellationToken.None);

        // Assert
        _loggerMock.Verify(
            expression: x => x.LogWarning(LoggingUtils.BackgroundJobWarning,
                "Skipping triggering events for job entry with Id {JobEntryId} because it's invalid. JobInstanceId: {JobInstanceId}",
                null, null, jobEntry.Id, _jobInstanceId), times: Times.Once);
    }

    [Test]
    public async Task Should_LogWarning_When_SerializationFailed()
    {
        // Arrange
        JobEntryTrigger trigger = new()
        {
            Id = Guid.NewGuid(),
            EventData = "7uU9Wpa",
            EventType = "Expenso.TimeManagement.Core.Domain.Jobs.Model.JobEntry"
        };

        JobEntry jobEntry = new()
        {
            Id = Guid.NewGuid(),
            Triggers = new List<JobEntryTrigger>
            {
                trigger
            }
        };

        _clockMock.Setup(expression: x => x.UtcNow).Returns(value: DateTimeOffset.UtcNow);

        _jobInstanceRepositoryMock
            .Setup(expression: x => x.GetAsync(_jobInstanceId, It.IsAny<CancellationToken>(), true))
            .ReturnsAsync(value: JobInstance.Default);

        _jobEntryRepositoryMock
            .Setup(expression: x => x.GetActiveJobEntries(It.IsAny<Guid>(), It.IsAny<CancellationToken>(), true))
            .ReturnsAsync(value: new List<JobEntry>
            {
                jobEntry
            });

        _serializerMock
            .Setup(expression: x => x.Deserialize(trigger.EventData, Type.GetType(trigger.EventType)!, null))
            .Returns(valueFunction: null!);

        // Act
        await TestCandidate.Execute(jobInstanceId: _jobInstanceId, interval: _interval,
            stoppingToken: CancellationToken.None);

        // Assert
        _loggerMock.Verify(
            expression: x => x.LogWarning(LoggingUtils.BackgroundJobWarning,
                "Failed to deserialize event trigger for job entry with Id {JobEntryId}. JobInstanceId: {JobInstanceId}",
                null, null, jobEntry.Id, _jobInstanceId), times: Times.Once);
    }

    [Test]
    public async Task Should_LogError_When_Error()
    {
        // Arrange
        Exception error = new(message: "This is test error");

        JobEntry jobEntry = new()
        {
            Id = Guid.NewGuid(),
            Triggers = new List<JobEntryTrigger>
            {
                new()
            }
        };

        _clockMock.Setup(expression: x => x.UtcNow).Returns(value: DateTimeOffset.UtcNow);

        _jobInstanceRepositoryMock
            .Setup(expression: x => x.GetAsync(_jobInstanceId, It.IsAny<CancellationToken>(), true))
            .Throws(exception: error);

        _jobEntryRepositoryMock
            .Setup(expression: x => x.GetActiveJobEntries(It.IsAny<Guid>(), It.IsAny<CancellationToken>(), true))
            .ReturnsAsync(value: new List<JobEntry>
            {
                jobEntry
            });

        // Act
        await TestCandidate.Execute(jobInstanceId: _jobInstanceId, interval: _interval,
            stoppingToken: CancellationToken.None);

        // Assert
        _loggerMock.Verify(
            expression: x => x.LogError(LoggingUtils.BackgroundJobError,
                "An error occurred while processing job. JobInstanceId: {JobInstanceId}", error, null, _jobInstanceId),
            times: Times.Once);
    }

    [Test]
    public async Task Should_PublishEvents_When_EverythingWentWell()
    {
        // Arrange
        BudgetPermissionRequestExpiredIntegrationEvent eventData =
            new(MessageContext: MessageContextFactoryMock.Object.Current(), BudgetPermissionRequestId: Guid.NewGuid());

        JobEntryTrigger trigger = new()
        {
            Id = Guid.NewGuid(),
            EventData = JsonSerializer.Serialize(value: eventData),
            EventType = $"{eventData.GetType().AssemblyQualifiedName}"
        };

        JobEntry jobEntry = new()
        {
            Id = Guid.NewGuid(),
            Triggers = new List<JobEntryTrigger>
            {
                trigger
            }
        };

        _clockMock.Setup(expression: x => x.UtcNow).Returns(value: DateTimeOffset.UtcNow);

        _jobInstanceRepositoryMock
            .Setup(expression: x => x.GetAsync(_jobInstanceId, It.IsAny<CancellationToken>(), true))
            .ReturnsAsync(value: JobInstance.Default);

        _jobEntryRepositoryMock
            .Setup(expression: x => x.GetActiveJobEntries(It.IsAny<Guid>(), It.IsAny<CancellationToken>(), true))
            .ReturnsAsync(value: new List<JobEntry>
            {
                jobEntry
            });

        _serializerMock
            .Setup(expression: x => x.Deserialize(trigger.EventData, Type.GetType(trigger.EventType)!,
                DefaultSerializerOptions.DefaultSettings))
            .Returns(value: eventData);

        // Act
        await TestCandidate.Execute(jobInstanceId: _jobInstanceId, interval: _interval,
            stoppingToken: CancellationToken.None);

        // Assert
        _loggerMock.Verify(
            expression: x => x.LogDebug(LoggingUtils.BackgroundJobGeneralInformation,
                "Published events: {Events} for job entry with Id {JobEntryId}. JobInstanceId: {JobInstanceId}", null,
                It.IsAny<object?>(), jobEntry.Id, _jobInstanceId), times: Times.Once);

        _messageBrokerMock.Verify(
            expression: x => x.PublishAsync<IIntegrationEvent>(eventData, It.IsAny<CancellationToken>()),
            times: Times.Once());

        jobEntry.LastRun.Should().NotBeNull();
    }

    [Test]
    public async Task Should_MakeJobFailed_When_MaxRetriesReached()
    {
        // Arrange
        Exception error = new(message: "This is test error");

        BudgetPermissionRequestExpiredIntegrationEvent eventData =
            new(MessageContext: MessageContextFactoryMock.Object.Current(), BudgetPermissionRequestId: Guid.NewGuid());

        JobEntryTrigger trigger = new()
        {
            Id = Guid.NewGuid(),
            EventData = JsonSerializer.Serialize(value: eventData),
            EventType = $"{typeof(BudgetPermissionRequestExpiredIntegrationEvent).AssemblyQualifiedName}"
        };

        JobEntry jobEntry = new()
        {
            Id = Guid.NewGuid(),
            Triggers = new List<JobEntryTrigger>
            {
                trigger
            },
            JobStatus = JobEntryStatus.Running
        };

        _clockMock.Setup(expression: x => x.UtcNow).Returns(value: DateTimeOffset.UtcNow);

        _jobInstanceRepositoryMock
            .Setup(expression: x => x.GetAsync(_jobInstanceId, It.IsAny<CancellationToken>(), true))
            .ReturnsAsync(value: JobInstance.Default);

        _jobEntryRepositoryMock
            .Setup(expression: x => x.GetActiveJobEntries(It.IsAny<Guid>(), It.IsAny<CancellationToken>(), true))
            .ReturnsAsync(value: new List<JobEntry>
            {
                jobEntry
            });

        _serializerMock
            .Setup(expression: x => x.Deserialize(trigger.EventData, Type.GetType(trigger.EventType)!,
                DefaultSerializerOptions.DefaultSettings))
            .Returns(value: eventData);

        _messageBrokerMock
            .Setup(expression: x => x.PublishAsync<IIntegrationEvent>(eventData, It.IsAny<CancellationToken>()))
            .Throws(exception: error);

        // Act
        await TestCandidate.Execute(jobInstanceId: _jobInstanceId, interval: _interval,
            stoppingToken: CancellationToken.None);

        // Assert
        _loggerMock.Verify(
            expression: x => x.LogError(LoggingUtils.BackgroundJobError,
                "An error occurred while processing and job entry with Id {JobEntryId}. JobInstanceId: {JobInstanceId}",
                error, null, jobEntry.Id, _jobInstanceId), times: Times.Once);

        jobEntry.CurrentRetries.Should().Be(expected: null);
        jobEntry.JobStatus.Should().Be(expected: JobEntryStatus.Failed);
    }

    [Test]
    public async Task Should_MakeJobRetrying_When_MaxRetriesHasNotBeenReached()
    {
        // Arrange
        Exception error = new(message: "This is test error");

        BudgetPermissionRequestExpiredIntegrationEvent eventData =
            new(MessageContext: MessageContextFactoryMock.Object.Current(), BudgetPermissionRequestId: Guid.NewGuid());

        JobEntryTrigger trigger = new()
        {
            Id = Guid.NewGuid(),
            EventData = JsonSerializer.Serialize(value: eventData),
            EventType = $"{typeof(BudgetPermissionRequestExpiredIntegrationEvent).AssemblyQualifiedName}"
        };

        JobEntry jobEntry = new()
        {
            Id = Guid.NewGuid(),
            Triggers = new List<JobEntryTrigger>
            {
                trigger
            },
            JobStatus = JobEntryStatus.Running,
            MaxRetries = 3
        };

        _clockMock.Setup(expression: x => x.UtcNow).Returns(value: DateTimeOffset.UtcNow);

        _jobInstanceRepositoryMock
            .Setup(expression: x => x.GetAsync(_jobInstanceId, It.IsAny<CancellationToken>(), true))
            .ReturnsAsync(value: JobInstance.Default);

        _jobEntryRepositoryMock
            .Setup(expression: x => x.GetActiveJobEntries(It.IsAny<Guid>(), It.IsAny<CancellationToken>(), true))
            .ReturnsAsync(value: new List<JobEntry>
            {
                jobEntry
            });

        _serializerMock
            .Setup(expression: x => x.Deserialize(trigger.EventData, Type.GetType(trigger.EventType)!,
                DefaultSerializerOptions.DefaultSettings))
            .Returns(value: eventData);

        _messageBrokerMock
            .Setup(expression: x => x.PublishAsync<IIntegrationEvent>(eventData, It.IsAny<CancellationToken>()))
            .Throws(exception: error);

        // Act
        await TestCandidate.Execute(jobInstanceId: _jobInstanceId, interval: _interval,
            stoppingToken: CancellationToken.None);

        // Assert
        _loggerMock.Verify(
            expression: x => x.LogError(LoggingUtils.BackgroundJobError,
                "An error occurred while processing and job entry with Id {JobEntryId}. JobInstanceId: {JobInstanceId}",
                error, null, jobEntry.Id, _jobInstanceId), times: Times.Once);

        jobEntry.CurrentRetries.Should().Be(expected: null);
        jobEntry.JobStatus.Should().Be(expected: JobEntryStatus.Retrying);
    }
}