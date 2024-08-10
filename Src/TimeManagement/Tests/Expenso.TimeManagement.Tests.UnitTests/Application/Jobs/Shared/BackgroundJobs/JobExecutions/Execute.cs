using Expenso.BudgetSharing.Proxy.DTO.MessageBus.BudgetPermissionRequests;
using Expenso.Shared.Integration.Events;
using Expenso.TimeManagement.Core.Domain.Jobs.Model;

using FluentAssertions;

using Microsoft.Extensions.Logging;

using Moq;

using Newtonsoft.Json;

using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Expenso.TimeManagement.Tests.UnitTests.Application.Jobs.Shared.BackgroundJobs.JobExecutions;

internal sealed class Execute : JobExecutionTestBase
{
    private readonly Guid _jobInstanceId = JobInstance.Default.Id;
    private readonly TimeSpan _interval = TimeSpan.FromSeconds(10);

    [Test]
    public async Task Should_LogInformation_When_JobInstanceHasNotBeenExists()
    {
        // Arrange
        string message = $"Job instance with id: {_jobInstanceId} hasn't been found";

        _jobInstanceRepositoryMock
            .Setup(x => x.GetAsync(_jobInstanceId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((JobInstance?)null);

        // Act
        await TestCandidate.Execute(_jobInstanceId, _interval, CancellationToken.None);

        // Assert
        _loggerMock.Verify(
            l => l.Log(LogLevel.Information, It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, _) => v.ToString()!.StartsWith(message)), It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()!), Times.Once);
    }

    [Test]
    public async Task Should_LogInformation_When_NoActiveJobEntriesFound()
    {
        // Arrange
        string message = $"No active job entries found. JobInstanceId: {_jobInstanceId}";

        _jobInstanceRepositoryMock
            .Setup(x => x.GetAsync(_jobInstanceId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(JobInstance.Default);

        _jobEntryRepositoryMock
            .Setup(x => x.GetActiveJobEntries(It.IsAny<Guid>(), It.IsAny<CancellationToken>(), false))
            .ReturnsAsync(new List<JobEntry>());

        // Act
        await TestCandidate.Execute(_jobInstanceId, _interval, CancellationToken.None);

        // Assert
        _loggerMock.Verify(
            l => l.Log(LogLevel.Information, It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, _) => v.ToString()!.StartsWith(message)), It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()!), Times.Once);
    }

    [Test]
    public async Task Should_LogWarning_When_NoJobEntryStatusesFound()
    {
        // Arrange
        const string message = "No job entry statuses found";

        _jobInstanceRepositoryMock
            .Setup(x => x.GetAsync(_jobInstanceId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(JobInstance.Default);

        _jobEntryRepositoryMock
            .Setup(x => x.GetActiveJobEntries(It.IsAny<Guid>(), It.IsAny<CancellationToken>(), false))
            .ReturnsAsync(new List<JobEntry>
            {
                new()
            });

        _jobEntryStatusRepositoryMock
            .Setup(x => x.GetAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<JobEntryStatus>());

        // Act
        await TestCandidate.Execute(_jobInstanceId, _interval, CancellationToken.None);

        // Assert
        _loggerMock.Verify(
            l => l.Log(LogLevel.Warning, It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, _) => v.ToString()!.StartsWith(message)), It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()!), Times.Once);
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
            .Setup(x => x.GetAsync(_jobInstanceId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(JobInstance.Default);

        _jobEntryRepositoryMock
            .Setup(x => x.GetActiveJobEntries(It.IsAny<Guid>(), It.IsAny<CancellationToken>(), false))
            .ReturnsAsync(new List<JobEntry>
            {
                jobEntry
            });

        string message =
            $"Skipping job entry with id {jobEntry.Id} because it has no triggers. JobInstanceId: {_jobInstanceId}";

        // Act
        await TestCandidate.Execute(_jobInstanceId, _interval, CancellationToken.None);

        // Assert
        _loggerMock.Verify(
            l => l.Log(LogLevel.Warning, It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, _) => v.ToString()!.StartsWith(message)), It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()!), Times.Once);
    }

    [Test]
    public async Task Should_SetJobAsCompleted_When_CronExpressionIsEmptyAndJobHasBeenRanBefore()
    {
        // Arrange
        var utcClock = DateTimeOffset.UtcNow;

        JobEntry jobEntry = new()
        {
            Id = Guid.NewGuid(),
            LastRun = utcClock.AddMinutes(-15),
            Triggers = new List<JobEntryTrigger>
            {
                new()
            }
        };

        _clockMock.Setup(x => x.UtcNow).Returns(DateTimeOffset.UtcNow);

        _jobInstanceRepositoryMock
            .Setup(x => x.GetAsync(_jobInstanceId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(JobInstance.Default);

        _jobEntryRepositoryMock
            .Setup(x => x.GetActiveJobEntries(It.IsAny<Guid>(), It.IsAny<CancellationToken>(), false))
            .ReturnsAsync(new List<JobEntry>
            {
                jobEntry
            });

        string message = $"Skipping job entry with id {jobEntry.Id} because it ended. JobInstanceId: {_jobInstanceId}";

        // Act
        await TestCandidate.Execute(_jobInstanceId, _interval, CancellationToken.None);

        // Assert
        _jobEntryRepositoryMock.Verify(
            x => x.AddOrUpdateAsync(It.Is<JobEntry>(j => j.Id == jobEntry.Id), It.IsAny<CancellationToken>()),
            Times.Once);

        jobEntry.IsCompleted.Should().BeTrue();

        _loggerMock.Verify(
            l => l.Log(LogLevel.Warning, It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, _) => v.ToString()!.StartsWith(message)), It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()!), Times.Once);
    }

    [Test]
    public async Task Should_SetJobAsCompleted_When_PeriodicJobWontRun()
    {
        // Arrange
        var utcClock = DateTimeOffset.UtcNow;

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
            .Setup(x => x.UtcNow)
            .Returns(new DateTime(utcClock.Year, utcClock.Month, utcClock.Day, utcClock.Hour, utcClock.Minute, 30,
                DateTimeKind.Utc));

        _jobInstanceRepositoryMock
            .Setup(x => x.GetAsync(_jobInstanceId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(JobInstance.Default);

        _jobEntryRepositoryMock
            .Setup(x => x.GetActiveJobEntries(It.IsAny<Guid>(), It.IsAny<CancellationToken>(), false))
            .ReturnsAsync(new List<JobEntry>
            {
                jobEntry
            });

        string message =
            $"Skipping job entry with Id {jobEntry.Id} because it's out of the actual run. JobInstanceId: {_jobInstanceId}";

        // Act
        await TestCandidate.Execute(_jobInstanceId, _interval, CancellationToken.None);

        // Assert
        _loggerMock.Verify(
            l => l.Log(LogLevel.Debug, It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, _) => v.ToString()!.StartsWith(message)), It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()!), Times.Once);
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

        _clockMock.Setup(x => x.UtcNow).Returns(DateTimeOffset.UtcNow);

        _jobInstanceRepositoryMock
            .Setup(x => x.GetAsync(_jobInstanceId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(JobInstance.Default);

        _jobEntryRepositoryMock
            .Setup(x => x.GetActiveJobEntries(It.IsAny<Guid>(), It.IsAny<CancellationToken>(), false))
            .ReturnsAsync(new List<JobEntry>
            {
                jobEntry
            });

        string message =
            $"Skipping triggering events for job entry with Id {jobEntry.Id} because it's invalid. JobInstanceId: {_jobInstanceId}";

        // Act
        await TestCandidate.Execute(_jobInstanceId, _interval, CancellationToken.None);

        // Assert
        _loggerMock.Verify(
            l => l.Log(LogLevel.Warning, It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, _) => v.ToString()!.StartsWith(message)), It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()!), Times.Once);
    }

    [Test]
    public async Task Should_LogWarning_When_SerializationFailed()
    {
        // Arrange
        var trigger = new JobEntryTrigger
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

        _clockMock.Setup(x => x.UtcNow).Returns(DateTimeOffset.UtcNow);

        _jobInstanceRepositoryMock
            .Setup(x => x.GetAsync(_jobInstanceId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(JobInstance.Default);

        _jobEntryRepositoryMock
            .Setup(x => x.GetActiveJobEntries(It.IsAny<Guid>(), It.IsAny<CancellationToken>(), false))
            .ReturnsAsync(new List<JobEntry>
            {
                jobEntry
            });

        _serializerMock
            .Setup(x => x.Deserialize(trigger.EventData, Type.GetType(trigger.EventType)!, null))
            .Returns(null!);

        string message =
            $"Failed to deserialize event trigger for job entry with Id {jobEntry.Id}. JobInstanceId: {_jobInstanceId}";

        // Act
        await TestCandidate.Execute(_jobInstanceId, _interval, CancellationToken.None);

        // Assert
        _loggerMock.Verify(
            l => l.Log(LogLevel.Warning, It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, _) => v.ToString()!.StartsWith(message)), It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()!), Times.Once);
    }

    [Test]
    public async Task Should_LogError_When_Error()
    {
        // Arrange
        var error = new Exception("This is test error");

        JobEntry jobEntry = new()
        {
            Id = Guid.NewGuid(),
            Triggers = new List<JobEntryTrigger>
            {
                new()
            }
        };

        _clockMock.Setup(x => x.UtcNow).Returns(DateTimeOffset.UtcNow);
        _jobInstanceRepositoryMock.Setup(x => x.GetAsync(_jobInstanceId, It.IsAny<CancellationToken>())).Throws(error);

        _jobEntryRepositoryMock
            .Setup(x => x.GetActiveJobEntries(It.IsAny<Guid>(), It.IsAny<CancellationToken>(), false))
            .ReturnsAsync(new List<JobEntry>
            {
                jobEntry
            });

        string message = $"An error occurred while processing job. JobInstanceId: {_jobInstanceId}";

        // Act
        await TestCandidate.Execute(_jobInstanceId, _interval, CancellationToken.None);

        // Assert
        _loggerMock.Verify(
            l => l.Log(LogLevel.Error, It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, _) => v.ToString()!.StartsWith(message)), error,
                It.IsAny<Func<It.IsAnyType, Exception, string>>()!), Times.Once);
    }

    [Test]
    public async Task Should_PublishEvents_When_EverythingWentWell()
    {
        // Arrange
        BudgetPermissionRequestExpiredIntergrationEvent eventData = new(MessageContextFactoryMock.Object.Current(),
            Guid.NewGuid());

        var trigger = new JobEntryTrigger
        {
            Id = Guid.NewGuid(),
            EventData = JsonSerializer.Serialize(eventData),
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

        _clockMock.Setup(x => x.UtcNow).Returns(DateTimeOffset.UtcNow);

        _jobInstanceRepositoryMock
            .Setup(x => x.GetAsync(_jobInstanceId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(JobInstance.Default);

        _jobEntryRepositoryMock
            .Setup(x => x.GetActiveJobEntries(It.IsAny<Guid>(), It.IsAny<CancellationToken>(), false))
            .ReturnsAsync(new List<JobEntry>
            {
                jobEntry
            });

        _serializerMock
            .Setup(x => x.Deserialize(trigger.EventData, Type.GetType(trigger.EventType)!, null))
            .Returns(eventData);

        string message =
            $"Published events: {string.Join(",", jobEntry.Triggers.Select(x => Type.GetType(x.EventType!)?.FullName))} for job entry with Id {jobEntry.Id}. JobInstanceId: {_jobInstanceId}";

        // Act
        await TestCandidate.Execute(_jobInstanceId, _interval, CancellationToken.None);

        // Assert
        _loggerMock.Verify(
            l => l.Log(LogLevel.Debug, It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, _) => v.ToString()!.StartsWith(message)), It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()!), Times.Once);

        _messageBrokerMock.Verify(x => x.PublishAsync<IIntegrationEvent>(eventData, It.IsAny<CancellationToken>()),
            Times.Once());

        jobEntry.LastRun.Should().NotBeNull();
    }

    [Test]
    public async Task Should_MakeJobFailed_When_MaxRetriesReached()
    {
        // Arrange
        var error = new Exception("This is test error");

        var eventData =
            new BudgetPermissionRequestExpiredIntergrationEvent(MessageContextFactoryMock.Object.Current(),
                Guid.NewGuid());

        var trigger = new JobEntryTrigger
        {
            Id = Guid.NewGuid(),
            EventData = JsonSerializer.Serialize(eventData),
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
        };

        _clockMock.Setup(x => x.UtcNow).Returns(DateTimeOffset.UtcNow);

        _jobInstanceRepositoryMock
            .Setup(x => x.GetAsync(_jobInstanceId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(JobInstance.Default);

        _jobEntryRepositoryMock
            .Setup(x => x.GetActiveJobEntries(It.IsAny<Guid>(), It.IsAny<CancellationToken>(), false))
            .ReturnsAsync(new List<JobEntry>
            {
                jobEntry
            });

        _serializerMock
            .Setup(x => x.Deserialize(trigger.EventData, Type.GetType(trigger.EventType)!, null))
            .Returns(eventData);

        _messageBrokerMock
            .Setup(x => x.PublishAsync<IIntegrationEvent>(eventData, It.IsAny<CancellationToken>()))
            .Throws(error);

        string message =
            $"An error occurred while processing and job entry with Id {jobEntry.Id}. JobInstanceId: {_jobInstanceId}";

        // Act
        await TestCandidate.Execute(_jobInstanceId, _interval, CancellationToken.None);

        // Assert
        _loggerMock.Verify(
            l => l.Log(LogLevel.Error, It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, _) => v.ToString()!.StartsWith(message)), It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()!), Times.Once);

        jobEntry.CurrentRetries.Should().Be(null);
        jobEntry.JobStatus.Should().Be(JobEntryStatus.Failed);
    }

    [Test]
    public async Task Should_MakeJobRetrying_When_MaxRetriesHasNotBeenReached()
    {
        // Arrange
        var error = new Exception("This is test error");

        var eventData =
            new BudgetPermissionRequestExpiredIntergrationEvent(MessageContextFactoryMock.Object.Current(),
                Guid.NewGuid());

        var trigger = new JobEntryTrigger
        {
            Id = Guid.NewGuid(),
            EventData = JsonSerializer.Serialize(eventData),
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

        _clockMock.Setup(x => x.UtcNow).Returns(DateTimeOffset.UtcNow);

        _jobInstanceRepositoryMock
            .Setup(x => x.GetAsync(_jobInstanceId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(JobInstance.Default);

        _jobEntryRepositoryMock
            .Setup(x => x.GetActiveJobEntries(It.IsAny<Guid>(), It.IsAny<CancellationToken>(), false))
            .ReturnsAsync(new List<JobEntry>
            {
                jobEntry
            });

        _serializerMock
            .Setup(x => x.Deserialize(trigger.EventData, Type.GetType(trigger.EventType)!, null))
            .Returns(eventData);

        _messageBrokerMock
            .Setup(x => x.PublishAsync<IIntegrationEvent>(eventData, It.IsAny<CancellationToken>()))
            .Throws(error);

        string message =
            $"An error occurred while processing and job entry with Id {jobEntry.Id}. JobInstanceId: {_jobInstanceId}";

        // Act
        await TestCandidate.Execute(_jobInstanceId, _interval, CancellationToken.None);

        // Assert
        _loggerMock.Verify(
            l => l.Log(LogLevel.Error, It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, _) => v.ToString()!.StartsWith(message)), It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()!), Times.Once);

        jobEntry.CurrentRetries.Should().Be(null);
        jobEntry.JobStatus.Should().Be(JobEntryStatus.Retrying);
    }
}