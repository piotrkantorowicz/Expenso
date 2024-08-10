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
            .Setup(x => x.GetAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(JobInstance.Default);

        _jobEntryStatusReposiotry
            .Setup(x => x.GetAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(JobEntryStatus.Running);

        // Act
        await TestCandidate.HandleAsync(_registerJobCommand, It.IsAny<CancellationToken>());

        // Assert
        _jobEntryRepositoryMock.Verify(x => x.AddOrUpdateAsync(It.IsAny<JobEntry>(), It.IsAny<CancellationToken>()));
    }

    [Test]
    public void Should_ThrowNoFoundException_When_JobInstanceNotFound()
    {
        // Arrange
        _jobInstanceRepository
            .Setup(x => x.GetAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((JobInstance?)null);

        // Act
        // Assert
        NotFoundException? exception = Assert.ThrowsAsync<NotFoundException>(() =>
            TestCandidate.HandleAsync(_registerJobCommand, It.IsAny<CancellationToken>()));

        string expectedExceptionMessage = $"Job instance with id {JobInstance.Default.Id} not found.";
        exception?.Message.Should().Be(expectedExceptionMessage);
    }

    [Test]
    public void Should_ThrowNoFoundException_When_JobRunningStatusNotFound()
    {
        // Arrange
        RegisterJobCommand command = new(MessageContextFactoryMock.Object.Current(), new AddJobEntryRequest(5, [
            new AddJobEntryRequest_JobEntryTrigger(
                typeof(BudgetPermissionRequestExpiredIntergrationEvent).AssemblyQualifiedName,
                _serializer.Object.Serialize(_eventTrigger))
        ], null, _clockMock.Object.UtcNow));

        _jobInstanceRepository
            .Setup(x => x.GetAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(JobInstance.Default);

        _jobEntryStatusReposiotry
            .Setup(x => x.GetAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((JobEntryStatus?)null);

        // Act
        // Assert
        NotFoundException? exception =
            Assert.ThrowsAsync<NotFoundException>(() =>
                TestCandidate.HandleAsync(command, It.IsAny<CancellationToken>()));

        string expectedExceptionMessage = $"Job status with id {JobEntryStatus.Running.Id} not found.";
        exception?.Message.Should().Be(expectedExceptionMessage);
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
            .Setup(x => x.GetAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(JobInstance.Default);

        _jobEntryStatusReposiotry
            .Setup(x => x.GetAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(JobEntryStatus.Running);

        // Act
        // Assert
        NotFoundException? exception =
            Assert.ThrowsAsync<NotFoundException>(() =>
                TestCandidate.HandleAsync(command, It.IsAny<CancellationToken>()));

        const string expectedExceptionMessage = "Unable to create job entry from request.";
        exception?.Message.Should().Be(expectedExceptionMessage);
    }
}