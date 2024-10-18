using System.Text.Json;

using Expenso.BudgetSharing.Shared.DTO.MessageBus.BudgetPermissionRequests;
using Expenso.Shared.Commands.Validation.Validators;
using Expenso.Shared.System.Serialization;
using Expenso.Shared.System.Serialization.Default;
using Expenso.Shared.System.Types.Clock;
using Expenso.Shared.System.Types.Messages.Interfaces;
using Expenso.Shared.Tests.Utils.UnitTests;
using Expenso.TimeManagement.Core.Application.Jobs.Shared.BackgroundJobs.Events;
using Expenso.TimeManagement.Core.Application.Jobs.Write.RegisterJob;
using Expenso.TimeManagement.Core.Application.Jobs.Write.RegisterJob.DTO.Request.Validators;
using Expenso.TimeManagement.Shared.DTO.Request;

using Moq;

namespace Expenso.TimeManagement.Tests.UnitTests.Application.Jobs.Write.RegisterJob.RegisterJobEntryCommandValidator;

[TestFixture]
internal abstract class
    RegisterJobEntryCommandValidatorTestBase : TestBase<
    Core.Application.Jobs.Write.RegisterJob.RegisterJobEntryCommandValidator>
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

        _eventTypeResolver = new Mock<IEventTypeResolver>();

        _eventTypeResolver
            .Setup(expression: x => x.IsAllowable(AllowedEvents.BudgetPermissionRequestExpired))
            .Returns(value: true);

        _eventTypeResolver
            .Setup(expression: x => x.Resolve(AllowedEvents.BudgetPermissionRequestExpired))
            .Returns(value: typeof(BudgetPermissionRequestExpiredIntegrationEvent));

        IMessageContext messageContext = MessageContextFactoryMock.Object.Current();
        string eventData = _serializer.Object.Serialize(value: eventTrigger);

        RegisterJobEntryRequest_JobEntryTrigger jobEntryTrigger =
            new(EventType: AllowedEvents.BudgetPermissionRequestExpired, EventData: eventData);

        RegisterJobEntryRequest payload = new(MaxRetries: 5, JobEntryTriggers: [jobEntryTrigger], Interval: null,
            RunAt: _clockMock.Object.UtcNow);

        _registerJobEntryCommand = new RegisterJobEntryCommand(MessageContext: messageContext, Payload: payload);
        MessageContextValidator messageContextValidator = new();
        RegisterJobEntryRequest_JobEntryPeriodIntervalValidator periodIntervalValidator = new();

        RegisterJobEntryRequest_JobEntryTriggerValidator triggerValidator =
            new(serializer: _serializer.Object, eventTypeResolver: _eventTypeResolver.Object);

        RegisterJobEntryRequestValidator requestValidator = new(
            jobEntryPeriodIntervalValidator: periodIntervalValidator, jobEntryTriggerValidator: triggerValidator,
            clock: _clockMock.Object);

        TestCandidate = new Core.Application.Jobs.Write.RegisterJob.RegisterJobEntryCommandValidator(
            messageContextValidator: messageContextValidator, registerJobEntryRequestValidator: requestValidator);
    }

    protected Mock<IClock> _clockMock = null!;
    protected RegisterJobEntryCommand _registerJobEntryCommand = null!;
    protected Mock<ISerializer> _serializer = null!;
    protected Mock<IEventTypeResolver> _eventTypeResolver = null!;
}