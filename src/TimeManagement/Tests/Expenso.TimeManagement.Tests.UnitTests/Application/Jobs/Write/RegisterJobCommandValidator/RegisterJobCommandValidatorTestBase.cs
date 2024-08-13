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
    protected RegisterJobCommand _registerJobCommand = null!;
    protected Mock<ISerializer> _serializer = null!;

    [SetUp]
    public void SetUp()
    {
        BudgetPermissionRequestExpiredIntergrationEvent eventTrigger = new(MessageContext: null!,
            BudgetPermissionRequestId: Guid.NewGuid());

        string eventTriggerPayload = JsonSerializer.Serialize(value: eventTrigger);
        _clockMock = new Mock<IClock>();
        _clockMock.Setup(expression: x => x.UtcNow).Returns(value: DateTimeOffset.UtcNow);
        _serializer = new Mock<ISerializer>();
        _serializer.Setup(expression: x => x.Serialize(eventTrigger, null)).Returns(value: eventTriggerPayload);

        _serializer
            .Setup(expression: x => x.Deserialize(eventTriggerPayload, eventTrigger.GetType(), null))
            .Returns(value: eventTrigger);

        _registerJobCommand = new RegisterJobCommand(MessageContext: MessageContextFactoryMock.Object.Current(),
            RegisterJobEntryRequest: new RegisterJobEntryRequest(MaxRetries: 5, JobEntryTriggers:
            [
                new RegisterJobEntryRequest_JobEntryTrigger(
                    EventType: typeof(BudgetPermissionRequestExpiredIntergrationEvent).AssemblyQualifiedName,
                    EventData: _serializer.Object.Serialize(value: eventTrigger))
            ], Interval: null, RunAt: _clockMock.Object.UtcNow));

        TestCandidate = new TestCandidate(serializer: _serializer.Object);
    }
}