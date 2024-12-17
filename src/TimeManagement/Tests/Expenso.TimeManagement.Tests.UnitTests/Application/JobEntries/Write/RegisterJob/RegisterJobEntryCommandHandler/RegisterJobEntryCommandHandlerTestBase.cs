using System.Text.Json;

using Expenso.BudgetSharing.Shared.DTO.MessageBus.BudgetPermissionRequests.ExpireAssigningParticipant;
using Expenso.BudgetSharing.Shared.DTO.MessageBus.BudgetPermissionRequests.ExpireAssigningParticipant.Payload;
using Expenso.Shared.System.Serialization;
using Expenso.Shared.System.Types.Clock;
using Expenso.Shared.Tests.Utils.UnitTests;
using Expenso.TimeManagement.Core.Application.JobEntries.Shared.BackgroundJobs.Events;
using Expenso.TimeManagement.Core.Application.JobEntries.Write.RegisterJobEntry;
using Expenso.TimeManagement.Core.Application.Shared.Settings;
using Expenso.TimeManagement.Core.Domain.JobEntries.Repositories;
using Expenso.TimeManagement.Shared.DTO.RegisterJobEntry.Request;

using Moq;

using NUnit.Framework;

namespace Expenso.TimeManagement.Tests.UnitTests.Application.JobEntries.Write.RegisterJob.
    RegisterJobEntryCommandHandler;

[TestFixture]
internal abstract class
    RegisterJobEntryCommandHandlerTestBase : TestBase<
    Core.Application.JobEntries.Write.RegisterJobEntry.RegisterJobEntryCommandHandler>
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
            .Setup(expression: x => x.Resolve(It.IsAny<AllowedEventType>()))
            .Returns(value: typeof(BudgetPermissionRequestExpiredIntegrationEvent));

        _jobEntryId = Guid.NewGuid();

        _registerJobEntryCommand = new RegisterJobEntryCommand(
            MessageContext: MessageContextFactoryMock.Object.Current(), Payload: new RegisterJobEntryRequest(
                JobEntryId: _jobEntryId, MaxRetries: 5, JobEntryTriggers:
                [
                    new RegisterJobEntryRequestJobEntryTrigger(
                        EventType: RegisterJobEntryRequestJobEntryTriggerAllowedEventType
                            .BudgetPermissionRequestExpired,
                        EventData: _serializer.Object.Serialize(value: _eventTrigger))
                ], Interval: null, RunAt: _clockMock.Object.UtcNow));

        TestCandidate = new Core.Application.JobEntries.Write.RegisterJobEntry.RegisterJobEntryCommandHandler(
            jobEntryRepository: _jobEntryRepositoryMock.Object, jobInstanceRepository: _jobInstanceRepository.Object,
            jobEntryStatusRepository: _jobEntryStatusReposiotry.Object, eventTypeResolver: _eventTypeResolver.Object);
    }

    [TearDown]
    public void TearDown()
    {
        _jobEntryRepositoryMock.Reset();
        _jobEntryStatusReposiotry.Reset();
        _jobInstanceRepository.Reset();
        _clockMock.Reset();
        _serializer.Reset();
        _eventTypeResolver.Reset();
        _jobEntryRepositoryMock = null!;
        _jobEntryStatusReposiotry = null!;
        _jobInstanceRepository = null!;
        _clockMock = null!;
        _serializer = null!;
        _eventTypeResolver = null!;
        TestCandidate = null!;
    }

    protected Guid _jobEntryId;
    protected Mock<IClock> _clockMock = null!;
    protected BudgetPermissionRequestExpiredIntegrationEvent _eventTrigger = null!;
    protected Mock<IJobEntryRepository> _jobEntryRepositoryMock = null!;
    protected Mock<IJobEntryStatusRepository> _jobEntryStatusReposiotry = null!;
    protected Mock<IJobInstanceRepository> _jobInstanceRepository = null!;
    protected RegisterJobEntryCommand _registerJobEntryCommand = null!;
    protected Mock<ISerializer> _serializer = null!;
    private Mock<IEventTypeResolver> _eventTypeResolver = null!;
}