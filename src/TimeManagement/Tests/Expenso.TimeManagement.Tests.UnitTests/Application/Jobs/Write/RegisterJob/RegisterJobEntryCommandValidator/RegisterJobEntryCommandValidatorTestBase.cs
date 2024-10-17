using System.Text.Json;

using Expenso.BudgetSharing.Shared.DTO.MessageBus.BudgetPermissionRequests;
using Expenso.Shared.Commands.Validation;
using Expenso.Shared.System.Serialization;
using Expenso.Shared.System.Serialization.Default;
using Expenso.Shared.System.Types.Clock;
using Expenso.Shared.Tests.Utils.UnitTests;
using Expenso.TimeManagement.Core.Application.Jobs.Write.RegisterJob;
using Expenso.TimeManagement.Core.Application.Jobs.Write.RegisterJob.DTO.Request.Validators;
using Expenso.TimeManagement.Shared.DTO.Request;

using Moq;

using TestCandidate = Expenso.TimeManagement.Core.Application.Jobs.Write.RegisterJob.RegisterJobEntryCommandValidator;

namespace Expenso.TimeManagement.Tests.UnitTests.Application.Jobs.Write.RegisterJob.RegisterJobEntryCommandValidator;

[TestFixture]
internal abstract class RegisterJobEntryCommandValidatorTestBase : TestBase<TestCandidate>
{
    [SetUp]
    public void SetUp()
    {
        BudgetPermissionRequestExpiredIntegrationEvent eventTrigger = new(MessageContext: null!,
            BudgetPermissionRequestId: Guid.NewGuid());

        string eventTriggerPayload = JsonSerializer.Serialize(value: eventTrigger);
        _clockMock = new Mock<IClock>();
        _clockMock.Setup(expression: x => x.UtcNow).Returns(value: DateTimeOffset.UtcNow);
        _serializer = new Mock<ISerializer>();
        _serializer.Setup(expression: x => x.Serialize(eventTrigger, null)).Returns(value: eventTriggerPayload);

        _serializer
            .Setup(expression: x =>
                x.Deserialize(eventTriggerPayload, eventTrigger.GetType(), DefaultSerializerOptions.DefaultSettings))
            .Returns(value: eventTrigger);

        _registerJobEntryCommand = new RegisterJobEntryCommand(
            MessageContext: MessageContextFactoryMock.Object.Current(), Payload: new RegisterJobEntryRequest(
                MaxRetries: 5, JobEntryTriggers:
                [
                    new RegisterJobEntryRequest_JobEntryTrigger(
                        EventType: typeof(BudgetPermissionRequestExpiredIntegrationEvent).AssemblyQualifiedName,
                        EventData: _serializer.Object.Serialize(value: eventTrigger))
                ], Interval: null, RunAt: _clockMock.Object.UtcNow));

        TestCandidate = new TestCandidate(messageContextValidator: new MessageContextValidator(),
            registerJobEntryRequestValidator: new RegisterJobEntryRequestValidator(
                jobEntryPeriodIntervalValidator: new RegisterJobEntryRequest_JobEntryPeriodIntervalValidator(),
                jobEntryTriggerValidator: new RegisterJobEntryRequest_JobEntryTriggerValidator(
                    serializer: _serializer.Object), clock: _clockMock.Object));
    }

    protected Mock<IClock> _clockMock = null!;
    protected RegisterJobEntryCommand _registerJobEntryCommand = null!;
    protected Mock<ISerializer> _serializer = null!;
}