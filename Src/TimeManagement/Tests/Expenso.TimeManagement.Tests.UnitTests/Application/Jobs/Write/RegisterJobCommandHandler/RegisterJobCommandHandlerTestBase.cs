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
    protected Mock<IJobEntryRepository> _jobEntryRepositoryMock = null!;
    protected Mock<IJobEntryStatusRepository> _jobEntryStatusReposiotry = null!;
    protected Mock<IJobInstanceRepository> _jobInstanceRepository = null!;
    protected Mock<IClock> _clockMock = null!;
    protected Mock<ISerializer> _serializer = null!;
    protected BudgetPermissionRequestExpiredIntergrationEvent _eventTrigger = null!;
    protected RegisterJobCommand _registerJobCommand = null!;

    [SetUp]
    public void SetUp()
    {
        _jobEntryRepositoryMock = new Mock<IJobEntryRepository>();
        _jobEntryStatusReposiotry = new Mock<IJobEntryStatusRepository>();
        _jobInstanceRepository = new Mock<IJobInstanceRepository>();
        _eventTrigger = new(null!, Guid.NewGuid());
        string eventTriggerPayload = JsonSerializer.Serialize(_eventTrigger);
        _clockMock = new Mock<IClock>();
        _clockMock.Setup(x => x.UtcNow).Returns(DateTimeOffset.UtcNow);
        _serializer = new Mock<ISerializer>();
        _serializer.Setup(x => x.Serialize(_eventTrigger, null)).Returns(eventTriggerPayload);

        _registerJobCommand = new RegisterJobCommand(MessageContextFactoryMock.Object.Current(), new AddJobEntryRequest(
            5, [
                new AddJobEntryRequest_JobEntryTrigger(
                    typeof(BudgetPermissionRequestExpiredIntergrationEvent).AssemblyQualifiedName,
                    _serializer.Object.Serialize(_eventTrigger))
            ], null, _clockMock.Object.UtcNow));

        TestCandidate = new TestCandidate(_jobEntryRepositoryMock.Object, _jobInstanceRepository.Object,
            _jobEntryStatusReposiotry.Object);
    }
}