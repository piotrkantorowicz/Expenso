using Expenso.BudgetSharing.Proxy.DTO.MessageBus.BudgetPermissionRequests;
using Expenso.Shared.System.Types.Exceptions;
using Expenso.TimeManagement.Core.Application.Jobs.Write.RegisterJob;
using Expenso.TimeManagement.Core.Domain.Jobs.Model;
using Expenso.TimeManagement.Proxy.DTO.Request;
using Expenso.TimeManagement.Proxy.DTO.Response;

using FluentAssertions;

using Moq;

namespace Expenso.TimeManagement.Tests.UnitTests.Application.Jobs.Write.RegisterJobCommandHandler;

internal sealed class HandleAsync : RegisterJobCommandHandlerTestBase
{
    [Test]
    public async Task Should_RegisterJobEntry()
    {
        // Arrange
        _jobInstanceRepository
            .Setup(expression: x => x.GetAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(value: JobInstance.Default);

        _jobEntryStatusReposiotry
            .Setup(expression: x => x.GetAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(value: JobEntryStatus.Running);

        // Act
        await TestCandidate.HandleAsync(command: _registerJobCommand, cancellationToken: It.IsAny<CancellationToken>());

        // Assert
        _jobEntryRepositoryMock.Verify(expression: x =>
            x.AddOrUpdateAsync(It.IsAny<JobEntry>(), It.IsAny<CancellationToken>()));
    }

    [Test]
    public async Task Should_RegisterJobEntry_And_CorrectlySetInterval()
    {
        // Arrange
        _jobInstanceRepository
            .Setup(expression: x => x.GetAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(value: JobInstance.Default);

        _jobEntryStatusReposiotry
            .Setup(expression: x => x.GetAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(value: JobEntryStatus.Running);

        RegisterJobCommand command = _registerJobCommand with
        {
            RegisterJobEntryRequest = _registerJobCommand.RegisterJobEntryRequest! with
            {
                Interval = new RegisterJobEntryRequest_JobEntryPeriodInterval(DayOfWeek: 5, Month: 6, DayofMonth: 10,
                    Hour: 12, Minute: 30, Second: 30)
            }
        };

        // Act
        RegisterJobEntryResponse? result =
            await TestCandidate.HandleAsync(command: command, cancellationToken: It.IsAny<CancellationToken>());

        // Assert
        _jobEntryRepositoryMock.Verify(expression: x =>
            x.AddOrUpdateAsync(It.IsAny<JobEntry>(), It.IsAny<CancellationToken>()));

        result?.CronExpression.Should().Be(expected: "5 6 10 12 30 30");
    }

    [Test]
    public void Should_ThrowNoFoundException_When_JobInstanceNotFound()
    {
        // Arrange
        _jobInstanceRepository
            .Setup(expression: x => x.GetAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(value: null);

        // Act
        // Assert
        NotFoundException? exception = Assert.ThrowsAsync<NotFoundException>(code: () =>
            TestCandidate.HandleAsync(command: _registerJobCommand, cancellationToken: It.IsAny<CancellationToken>()));

        string expectedExceptionMessage = $"Job instance with id {JobInstance.Default.Id} not found";
        exception?.Message.Should().Be(expected: expectedExceptionMessage);
    }

    [Test]
    public void Should_ThrowNoFoundException_When_JobRunningStatusNotFound()
    {
        // Arrange
        RegisterJobCommand command = new(MessageContext: MessageContextFactoryMock.Object.Current(),
            RegisterJobEntryRequest: new RegisterJobEntryRequest(MaxRetries: 5, JobEntryTriggers:
            [
                new RegisterJobEntryRequest_JobEntryTrigger(
                    EventType: typeof(BudgetPermissionRequestExpiredIntegrationEvent).AssemblyQualifiedName,
                    EventData: _serializer.Object.Serialize(value: _eventTrigger))
            ], Interval: null, RunAt: _clockMock.Object.UtcNow));

        _jobInstanceRepository
            .Setup(expression: x => x.GetAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(value: JobInstance.Default);

        _jobEntryStatusReposiotry
            .Setup(expression: x => x.GetAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(value: null);

        // Act
        // Assert
        NotFoundException? exception = Assert.ThrowsAsync<NotFoundException>(code: () =>
            TestCandidate.HandleAsync(command: command, cancellationToken: It.IsAny<CancellationToken>()));

        string expectedExceptionMessage = $"Job status with id {JobEntryStatus.Running.Id} not found";
        exception?.Message.Should().Be(expected: expectedExceptionMessage);
    }

    [Test]
    public void Should_ThrowNoFoundException_When_RegisterJobEntryRequestIsNull()
    {
        // Arrange
        RegisterJobCommand command = _registerJobCommand with
        {
            RegisterJobEntryRequest = null
        };

        _jobInstanceRepository
            .Setup(expression: x => x.GetAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(value: JobInstance.Default);

        _jobEntryStatusReposiotry
            .Setup(expression: x => x.GetAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(value: JobEntryStatus.Running);

        // Act
        // Assert
        NotFoundException? exception = Assert.ThrowsAsync<NotFoundException>(code: () =>
            TestCandidate.HandleAsync(command: command, cancellationToken: It.IsAny<CancellationToken>()));

        const string expectedExceptionMessage = "Unable to create job entry from request";
        exception?.Message.Should().Be(expected: expectedExceptionMessage);
    }
}