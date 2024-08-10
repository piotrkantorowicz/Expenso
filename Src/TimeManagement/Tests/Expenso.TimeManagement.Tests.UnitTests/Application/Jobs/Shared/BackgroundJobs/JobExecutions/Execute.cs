using Expenso.BudgetSharing.Proxy.DTO.MessageBus.BudgetPermissionRequests;
using Expenso.Shared.Integration.Events;
using Expenso.TimeManagement.Core.Domain.Jobs.Model;

using FluentAssertions;

using Microsoft.Extensions.Logging;

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
        string message = $"Job instance with id: {_jobInstanceId} hasn't been found";

        _jobInstanceRepositoryMock
            .Setup(expression: x => x.GetAsync(_jobInstanceId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(value: (JobInstance?)null);

        // Act
        await TestCandidate.Execute(jobInstanceId: _jobInstanceId, interval: _interval,
            stoppingToken: CancellationToken.None);

        // Assert
        _loggerMock.Verify(
            expression: l => l.Log(LogLevel.Information, It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, _) => v.ToString()!.StartsWith(message)), It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()!), times: Times.Once);
    }

    [Test]
    public async Task Should_LogInformation_When_NoActiveJobEntriesFound()
    {
        // Arrange
        string message = $"No active job entries found. JobInstanceId: {_jobInstanceId}";

        _jobInstanceRepositoryMock
            .Setup(expression: x => x.GetAsync(_jobInstanceId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(value: JobInstance.Default);

        _jobEntryRepositoryMock
            .Setup(expression: x => x.GetActiveJobEntries(It.IsAny<Guid>(), It.IsAny<CancellationToken>(), false))
            .ReturnsAsync(value: new List<JobEntry>());

        // Act
        await TestCandidate.Execute(jobInstanceId: _jobInstanceId, interval: _interval,
            stoppingToken: CancellationToken.None);

        // Assert
        _loggerMock.Verify(
            expression: l => l.Log(LogLevel.Information, It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, _) => v.ToString()!.StartsWith(message)), It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()!), times: Times.Once);
    }

    [Test]
    public async Task Should_LogWarning_When_NoJobEntryStatusesFound()
    {
        // Arrange
        const string message = "No job entry statuses found";

        _jobInstanceRepositoryMock
            .Setup(expression: x => x.GetAsync(_jobInstanceId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(value: JobInstance.Default);

        _jobEntryRepositoryMock
            .Setup(expression: x => x.GetActiveJobEntries(It.IsAny<Guid>(), It.IsAny<CancellationToken>(), false))
            .ReturnsAsync(value: new List<JobEntry>
            {
                new()
            });

        _jobEntryStatusRepositoryMock
            .Setup(expression: x => x.GetAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(value: new List<JobEntryStatus>());

        // Act
        await TestCandidate.Execute(jobInstanceId: _jobInstanceId, interval: _interval,
            stoppingToken: CancellationToken.None);

        // Assert
        _loggerMock.Verify(
            expression: l => l.Log(LogLevel.Warning, It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, _) => v.ToString()!.StartsWith(message)), It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()!), times: Times.Once);
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
            .Setup(expression: x => x.GetAsync(_jobInstanceId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(value: JobInstance.Default);

        _jobEntryRepositoryMock
            .Setup(expression: x => x.GetActiveJobEntries(It.IsAny<Guid>(), It.IsAny<CancellationToken>(), false))
            .ReturnsAsync(value: new List<JobEntry>
            {
                jobEntry
            });

        string message =
            $"Skipping job entry with id {jobEntry.Id} because it has no triggers. JobInstanceId: {_jobInstanceId}";

        // Act
        await TestCandidate.Execute(jobInstanceId: _jobInstanceId, interval: _interval,
            stoppingToken: CancellationToken.None);

        // Assert
        _loggerMock.Verify(
            expression: l => l.Log(LogLevel.Warning, It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, _) => v.ToString()!.StartsWith(message)), It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()!), times: Times.Once);
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
            .Setup(expression: x => x.GetAsync(_jobInstanceId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(value: JobInstance.Default);

        _jobEntryRepositoryMock
            .Setup(expression: x => x.GetActiveJobEntries(It.IsAny<Guid>(), It.IsAny<CancellationToken>(), false))
            .ReturnsAsync(value: new List<JobEntry>
            {
                jobEntry
            });

        string message = $"Skipping job entry with id {jobEntry.Id} because it ended. JobInstanceId: {_jobInstanceId}";

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
            expression: l => l.Log(LogLevel.Warning, It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, _) => v.ToString()!.StartsWith(message)), It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()!), times: Times.Once);
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
            .Setup(expression: x => x.GetAsync(_jobInstanceId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(value: JobInstance.Default);

        _jobEntryRepositoryMock
            .Setup(expression: x => x.GetActiveJobEntries(It.IsAny<Guid>(), It.IsAny<CancellationToken>(), false))
            .ReturnsAsync(value: new List<JobEntry>
            {
                jobEntry
            });

        string message =
            $"Skipping job entry with Id {jobEntry.Id} because it's out of the actual run. JobInstanceId: {_jobInstanceId}";

        // Act
        await TestCandidate.Execute(jobInstanceId: _jobInstanceId, interval: _interval,
            stoppingToken: CancellationToken.None);

        // Assert
        _loggerMock.Verify(
            expression: l => l.Log(LogLevel.Debug, It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, _) => v.ToString()!.StartsWith(message)), It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()!), times: Times.Once);
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
            .Setup(expression: x => x.GetAsync(_jobInstanceId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(value: JobInstance.Default);

        _jobEntryRepositoryMock
            .Setup(expression: x => x.GetActiveJobEntries(It.IsAny<Guid>(), It.IsAny<CancellationToken>(), false))
            .ReturnsAsync(value: new List<JobEntry>
            {
                jobEntry
            });

        string message =
            $"Skipping triggering events for job entry with Id {jobEntry.Id} because it's invalid. JobInstanceId: {_jobInstanceId}";

        // Act
        await TestCandidate.Execute(jobInstanceId: _jobInstanceId, interval: _interval,
            stoppingToken: CancellationToken.None);

        // Assert
        _loggerMock.Verify(
            expression: l => l.Log(LogLevel.Warning, It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, _) => v.ToString()!.StartsWith(message)), It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()!), times: Times.Once);
    }

    [Test]
    public async Task Should_LogWarning_When_SerializationFailed()
    {
        // Arrange
        JobEntryTrigger trigger = new JobEntryTrigger
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
            .Setup(expression: x => x.GetAsync(_jobInstanceId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(value: JobInstance.Default);

        _jobEntryRepositoryMock
            .Setup(expression: x => x.GetActiveJobEntries(It.IsAny<Guid>(), It.IsAny<CancellationToken>(), false))
            .ReturnsAsync(value: new List<JobEntry>
            {
                jobEntry
            });

        _serializerMock
            .Setup(expression: x => x.Deserialize(trigger.EventData, Type.GetType(trigger.EventType)!, null))
            .Returns(valueFunction: null!);

        string message =
            $"Failed to deserialize event trigger for job entry with Id {jobEntry.Id}. JobInstanceId: {_jobInstanceId}";

        // Act
        await TestCandidate.Execute(jobInstanceId: _jobInstanceId, interval: _interval,
            stoppingToken: CancellationToken.None);

        // Assert
        _loggerMock.Verify(
            expression: l => l.Log(LogLevel.Warning, It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, _) => v.ToString()!.StartsWith(message)), It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()!), times: Times.Once);
    }

    [Test]
    public async Task Should_LogError_When_Error()
    {
        // Arrange
        Exception error = new Exception(message: "This is test error");

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
            .Setup(expression: x => x.GetAsync(_jobInstanceId, It.IsAny<CancellationToken>()))
            .Throws(exception: error);

        _jobEntryRepositoryMock
            .Setup(expression: x => x.GetActiveJobEntries(It.IsAny<Guid>(), It.IsAny<CancellationToken>(), false))
            .ReturnsAsync(value: new List<JobEntry>
            {
                jobEntry
            });

        string message = $"An error occurred while processing job. JobInstanceId: {_jobInstanceId}";

        // Act
        await TestCandidate.Execute(jobInstanceId: _jobInstanceId, interval: _interval,
            stoppingToken: CancellationToken.None);

        // Assert
        _loggerMock.Verify(
            expression: l => l.Log(LogLevel.Error, It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, _) => v.ToString()!.StartsWith(message)), error,
                It.IsAny<Func<It.IsAnyType, Exception, string>>()!), times: Times.Once);
    }

    [Test]
    public async Task Should_PublishEvents_When_EverythingWentWell()
    {
        // Arrange
        BudgetPermissionRequestExpiredIntergrationEvent eventData =
            new(MessageContext: MessageContextFactoryMock.Object.Current(), BudgetPermissionRequestId: Guid.NewGuid());

        JobEntryTrigger trigger = new JobEntryTrigger
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
            .Setup(expression: x => x.GetAsync(_jobInstanceId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(value: JobInstance.Default);

        _jobEntryRepositoryMock
            .Setup(expression: x => x.GetActiveJobEntries(It.IsAny<Guid>(), It.IsAny<CancellationToken>(), false))
            .ReturnsAsync(value: new List<JobEntry>
            {
                jobEntry
            });

        _serializerMock
            .Setup(expression: x => x.Deserialize(trigger.EventData, Type.GetType(trigger.EventType)!, null))
            .Returns(value: eventData);

        string message =
            $"Published events: {string.Join(separator: ",", values: jobEntry.Triggers.Select(selector: x => Type.GetType(typeName: x.EventType!)?.FullName))} for job entry with Id {jobEntry.Id}. JobInstanceId: {_jobInstanceId}";

        // Act
        await TestCandidate.Execute(jobInstanceId: _jobInstanceId, interval: _interval,
            stoppingToken: CancellationToken.None);

        // Assert
        _loggerMock.Verify(
            expression: l => l.Log(LogLevel.Debug, It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, _) => v.ToString()!.StartsWith(message)), It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()!), times: Times.Once);

        _messageBrokerMock.Verify(
            expression: x => x.PublishAsync<IIntegrationEvent>(eventData, It.IsAny<CancellationToken>()),
            times: Times.Once());

        jobEntry.LastRun.Should().NotBeNull();
    }

    [Test]
    public async Task Should_MakeJobFailed_When_MaxRetriesReached()
    {
        // Arrange
        Exception error = new Exception(message: "This is test error");

        BudgetPermissionRequestExpiredIntergrationEvent eventData =
            new BudgetPermissionRequestExpiredIntergrationEvent(
                MessageContext: MessageContextFactoryMock.Object.Current(), BudgetPermissionRequestId: Guid.NewGuid());

        JobEntryTrigger trigger = new JobEntryTrigger
        {
            Id = Guid.NewGuid(),
            EventData = JsonSerializer.Serialize(value: eventData),
            EventType = $"{typeof(BudgetPermissionRequestExpiredIntergrationEvent).AssemblyQualifiedName}"
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
            .Setup(expression: x => x.GetAsync(_jobInstanceId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(value: JobInstance.Default);

        _jobEntryRepositoryMock
            .Setup(expression: x => x.GetActiveJobEntries(It.IsAny<Guid>(), It.IsAny<CancellationToken>(), false))
            .ReturnsAsync(value: new List<JobEntry>
            {
                jobEntry
            });

        _serializerMock
            .Setup(expression: x => x.Deserialize(trigger.EventData, Type.GetType(trigger.EventType)!, null))
            .Returns(value: eventData);

        _messageBrokerMock
            .Setup(expression: x => x.PublishAsync<IIntegrationEvent>(eventData, It.IsAny<CancellationToken>()))
            .Throws(exception: error);

        string message =
            $"An error occurred while processing and job entry with Id {jobEntry.Id}. JobInstanceId: {_jobInstanceId}";

        // Act
        await TestCandidate.Execute(jobInstanceId: _jobInstanceId, interval: _interval,
            stoppingToken: CancellationToken.None);

        // Assert
        _loggerMock.Verify(
            expression: l => l.Log(LogLevel.Error, It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, _) => v.ToString()!.StartsWith(message)), It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()!), times: Times.Once);

        jobEntry.CurrentRetries.Should().Be(expected: null);
        jobEntry.JobStatus.Should().Be(expected: JobEntryStatus.Failed);
    }

    [Test]
    public async Task Should_MakeJobRetrying_When_MaxRetriesHasNotBeenReached()
    {
        // Arrange
        Exception error = new Exception(message: "This is test error");

        BudgetPermissionRequestExpiredIntergrationEvent eventData =
            new BudgetPermissionRequestExpiredIntergrationEvent(
                MessageContext: MessageContextFactoryMock.Object.Current(), BudgetPermissionRequestId: Guid.NewGuid());

        JobEntryTrigger trigger = new JobEntryTrigger
        {
            Id = Guid.NewGuid(),
            EventData = JsonSerializer.Serialize(value: eventData),
            EventType = $"{typeof(BudgetPermissionRequestExpiredIntergrationEvent).AssemblyQualifiedName}"
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
            .Setup(expression: x => x.GetAsync(_jobInstanceId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(value: JobInstance.Default);

        _jobEntryRepositoryMock
            .Setup(expression: x => x.GetActiveJobEntries(It.IsAny<Guid>(), It.IsAny<CancellationToken>(), false))
            .ReturnsAsync(value: new List<JobEntry>
            {
                jobEntry
            });

        _serializerMock
            .Setup(expression: x => x.Deserialize(trigger.EventData, Type.GetType(trigger.EventType)!, null))
            .Returns(value: eventData);

        _messageBrokerMock
            .Setup(expression: x => x.PublishAsync<IIntegrationEvent>(eventData, It.IsAny<CancellationToken>()))
            .Throws(exception: error);

        string message =
            $"An error occurred while processing and job entry with Id {jobEntry.Id}. JobInstanceId: {_jobInstanceId}";

        // Act
        await TestCandidate.Execute(jobInstanceId: _jobInstanceId, interval: _interval,
            stoppingToken: CancellationToken.None);

        // Assert
        _loggerMock.Verify(
            expression: l => l.Log(LogLevel.Error, It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, _) => v.ToString()!.StartsWith(message)), It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()!), times: Times.Once);

        jobEntry.CurrentRetries.Should().Be(expected: null);
        jobEntry.JobStatus.Should().Be(expected: JobEntryStatus.Retrying);
    }
}