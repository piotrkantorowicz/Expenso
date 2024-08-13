using System.Text.Json;

using Expenso.BudgetSharing.Proxy.DTO.MessageBus.BudgetPermissionRequests;
using Expenso.Shared.System.Serialization;
using Expenso.Shared.System.Types.Clock;
using Expenso.Shared.Tests.Utils.UnitTests;
using Expenso.TimeManagement.Core.Application.Jobs.Write.RegisterJob;
using Expenso.TimeManagement.Core.Domain.Jobs.Repositories;
using Expenso.TimeManagement.Proxy.DTO.Request;

using Moq;

using TestCandidate = Expenso.TimeManagement.Core.Application.Jobs.Write.RegisterJob.RegisterJobCommandHandler;

namespace Expenso.TimeManagement.Tests.UnitTests.Application.Jobs.Write.RegisterJobCommandHandler;

internal abstract class RegisterJobCommandHandlerTestBase : TestBase<TestCandidate>
{
    protected Mock<IClock> _clockMock = null!;
    protected BudgetPermissionRequestExpiredIntergrationEvent _eventTrigger = null!;
    protected Mock<IJobEntryRepository> _jobEntryRepositoryMock = null!;
    protected Mock<IJobEntryStatusRepository> _jobEntryStatusReposiotry = null!;
    protected Mock<IJobInstanceRepository> _jobInstanceRepository = null!;
    protected RegisterJobCommand _registerJobCommand = null!;
    protected Mock<ISerializer> _serializer = null!;

    [SetUp]
    public void SetUp()
    {
        _jobEntryRepositoryMock = new Mock<IJobEntryRepository>();
        _jobEntryStatusReposiotry = new Mock<IJobEntryStatusRepository>();
        _jobInstanceRepository = new Mock<IJobInstanceRepository>();

        _eventTrigger =
            new BudgetPermissionRequestExpiredIntergrationEvent(MessageContext: null!,
                BudgetPermissionRequestId: Guid.NewGuid());

        string eventTriggerPayload = JsonSerializer.Serialize(value: _eventTrigger);
        _clockMock = new Mock<IClock>();
        _clockMock.Setup(expression: x => x.UtcNow).Returns(value: DateTimeOffset.UtcNow);
        _serializer = new Mock<ISerializer>();
        _serializer.Setup(expression: x => x.Serialize(_eventTrigger, null)).Returns(value: eventTriggerPayload);

        _registerJobCommand = new RegisterJobCommand(MessageContext: MessageContextFactoryMock.Object.Current(),
            RegisterJobEntryRequest: new RegisterJobEntryRequest(MaxRetries: 5, JobEntryTriggers:
            [
                new RegisterJobEntryRequest_JobEntryTrigger(
                    EventType: typeof(BudgetPermissionRequestExpiredIntergrationEvent).AssemblyQualifiedName,
                    EventData: _serializer.Object.Serialize(value: _eventTrigger))
            ], Interval: null, RunAt: _clockMock.Object.UtcNow));

        TestCandidate = new TestCandidate(jobEntryRepository: _jobEntryRepositoryMock.Object,
            jobInstanceRepository: _jobInstanceRepository.Object,
            jobEntryStatusRepository: _jobEntryStatusReposiotry.Object);
    }
}