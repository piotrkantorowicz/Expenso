using Expenso.BudgetSharing.Proxy.DTO.MessageBus.BudgetPermissionRequests;
using Expenso.Shared.System.Types.Exceptions;
using Expenso.TimeManagement.Core.Application.Jobs.Write.RegisterJob;
using Expenso.TimeManagement.Core.Domain.Jobs.Model;
using Expenso.TimeManagement.Proxy.DTO.Request;

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

        string expectedExceptionMessage = $"Job instance with id {JobInstance.Default.Id} not found.";
        exception?.Message.Should().Be(expected: expectedExceptionMessage);
    }

    [Test]
    public void Should_ThrowNoFoundException_When_JobRunningStatusNotFound()
    {
        // Arrange
        RegisterJobCommand command = new(MessageContext: MessageContextFactoryMock.Object.Current(),
            AddJobEntryRequest: new AddJobEntryRequest(MaxRetries: 5, JobEntryTriggers:
            [
                new AddJobEntryRequest_JobEntryTrigger(
                    EventType: typeof(BudgetPermissionRequestExpiredIntergrationEvent).AssemblyQualifiedName,
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

        string expectedExceptionMessage = $"Job status with id {JobEntryStatus.Running.Id} not found.";
        exception?.Message.Should().Be(expected: expectedExceptionMessage);
    }

    [Test]
    public void Should_ThrowNoFoundException_When_AddJobEntryRequestIsNull()
    {
        // Arrange
        RegisterJobCommand command = _registerJobCommand with
        {
            AddJobEntryRequest = null
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

        const string expectedExceptionMessage = "Unable to create job entry from request.";
        exception?.Message.Should().Be(expected: expectedExceptionMessage);
    }
}