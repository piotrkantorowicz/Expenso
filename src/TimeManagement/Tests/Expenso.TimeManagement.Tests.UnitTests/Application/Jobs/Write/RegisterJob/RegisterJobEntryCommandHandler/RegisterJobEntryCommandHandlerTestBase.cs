using System.Text.Json;

using Expenso.BudgetSharing.Shared.DTO.MessageBus.BudgetPermissionRequests.ExpireAssigningParticipant;
using Expenso.BudgetSharing.Shared.DTO.MessageBus.BudgetPermissionRequests.ExpireAssigningParticipant.Payload;
using Expenso.Shared.System.Serialization;
using Expenso.Shared.System.Types.Clock;
using Expenso.Shared.Tests.Utils.UnitTests;
using Expenso.TimeManagement.Core.Application.Jobs.Shared.BackgroundJobs.Events;
using Expenso.TimeManagement.Core.Application.Jobs.Write.RegisterJob;
using Expenso.TimeManagement.Core.Domain.Jobs.Repositories;
using Expenso.TimeManagement.Shared.DTO.Request;

using Moq;

namespace Expenso.TimeManagement.Tests.UnitTests.Application.Jobs.Write.RegisterJob.RegisterJobEntryCommandHandler;

[TestFixture]
internal abstract class
    RegisterJobEntryCommandHandlerTestBase : TestBase<
    Core.Application.Jobs.Write.RegisterJob.RegisterJobEntryCommandHandler>
{
    [SetUp]
    public void SetUp()
    {
        _jobEntryRepositoryMock = new Mock<IJobEntryRepository>();
        _jobEntryStatusReposiotry = new Mock<IJobEntryStatusRepository>();
        _jobInstanceRepository = new Mock<IJobInstanceRepository>();

        _eventTrigger = new BudgetPermissionRequestExpiredIntegrationEvent(
            MessageContext: MessageContextFactoryMock.Object.Current(),
            Payload: new BudgetPermissionRequestExpiredPayload(BudgetPermissionRequestId: Guid.NewGuid()));

        string eventTriggerPayload = JsonSerializer.Serialize(value: _eventTrigger);
        _clockMock = new Mock<IClock>();
        _clockMock.Setup(expression: x => x.UtcNow).Returns(value: DateTimeOffset.UtcNow);
        _serializer = new Mock<ISerializer>();
        _serializer.Setup(expression: x => x.Serialize(_eventTrigger, null)).Returns(value: eventTriggerPayload);
        _eventTypeResolver = new Mock<IEventTypeResolver>();

        _eventTypeResolver
            .Setup(expression: x => x.Resolve(It.IsAny<string>()))
            .Returns(value: typeof(BudgetPermissionRequestExpiredIntegrationEvent));

        _registerJobEntryCommand = new RegisterJobEntryCommand(
            MessageContext: MessageContextFactoryMock.Object.Current(), Payload: new RegisterJobEntryRequest(
                MaxRetries: 5, JobEntryTriggers:
                [
                    new RegisterJobEntryRequest_JobEntryTrigger(
                        EventType: typeof(BudgetPermissionRequestExpiredIntegrationEvent).AssemblyQualifiedName,
                        EventData: _serializer.Object.Serialize(value: _eventTrigger))
                ], Interval: null, RunAt: _clockMock.Object.UtcNow));

        TestCandidate = new Core.Application.Jobs.Write.RegisterJob.RegisterJobEntryCommandHandler(
            jobEntryRepository: _jobEntryRepositoryMock.Object, jobInstanceRepository: _jobInstanceRepository.Object,
            jobEntryStatusRepository: _jobEntryStatusReposiotry.Object, eventTypeResolver: _eventTypeResolver.Object);
    }

    protected Mock<IClock> _clockMock = null!;
    protected BudgetPermissionRequestExpiredIntegrationEvent _eventTrigger = null!;
    protected Mock<IJobEntryRepository> _jobEntryRepositoryMock = null!;
    protected Mock<IJobEntryStatusRepository> _jobEntryStatusReposiotry = null!;
    protected Mock<IJobInstanceRepository> _jobInstanceRepository = null!;
    protected RegisterJobEntryCommand _registerJobEntryCommand = null!;
    protected Mock<ISerializer> _serializer = null!;
    protected Mock<IEventTypeResolver> _eventTypeResolver = null!;
}