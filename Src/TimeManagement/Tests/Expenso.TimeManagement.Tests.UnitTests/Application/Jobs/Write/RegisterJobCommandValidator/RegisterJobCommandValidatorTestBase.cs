using System.Text.Json;

using Expenso.BudgetSharing.Proxy.DTO.MessageBus.BudgetPermissionRequests;
using Expenso.Shared.System.Serialization;
using Expenso.Shared.System.Types.Clock;
using Expenso.Shared.Tests.Utils.UnitTests;
using Expenso.TimeManagement.Core.Application.Jobs.Write.RegisterJob;
using Expenso.TimeManagement.Proxy.DTO.Request;

using Moq;

using TestCandidate = Expenso.TimeManagement.Core.Application.Jobs.Write.RegisterJob.RegisterJobCommandValidator;

namespace Expenso.TimeManagement.Tests.UnitTests.Application.Jobs.Write.RegisterJobCommandValidator;

internal abstract class RegisterJobCommandValidatorTestBase : TestBase<TestCandidate>
{
    protected Mock<IClock> _clockMock = null!;
    protected Mock<ISerializer> _serializer = null!;
    protected RegisterJobCommand _registerJobCommand = null!;

    [SetUp]
    public void SetUp()
    {
        BudgetPermissionRequestExpiredIntergrationEvent eventTrigger = new(null!, Guid.NewGuid());
        string eventTriggerPayload = JsonSerializer.Serialize(eventTrigger);
        _clockMock = new Mock<IClock>();
        _clockMock.Setup(x => x.UtcNow).Returns(DateTimeOffset.UtcNow);
        _serializer = new Mock<ISerializer>();
        _serializer.Setup(x => x.Serialize(eventTrigger, null)).Returns(eventTriggerPayload);

        _serializer
            .Setup(x => x.Deserialize(eventTriggerPayload, eventTrigger.GetType(), null))
            .Returns(eventTrigger);

        _registerJobCommand = new RegisterJobCommand(MessageContextFactoryMock.Object.Current(), new AddJobEntryRequest(
            5, [
                new AddJobEntryRequest_JobEntryTrigger(
                    typeof(BudgetPermissionRequestExpiredIntergrationEvent).AssemblyQualifiedName,
                    _serializer.Object.Serialize(eventTrigger))
            ], null, _clockMock.Object.UtcNow));

        TestCandidate = new TestCandidate(_serializer.Object);
    }
}