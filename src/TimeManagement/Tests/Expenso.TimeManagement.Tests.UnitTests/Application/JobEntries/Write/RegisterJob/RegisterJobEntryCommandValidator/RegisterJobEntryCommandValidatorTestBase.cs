using System.Text.Json;

using Expenso.BudgetSharing.Shared.DTO.MessageBus.BudgetPermissionRequests.ExpireAssigningParticipant;
using Expenso.BudgetSharing.Shared.DTO.MessageBus.BudgetPermissionRequests.ExpireAssigningParticipant.Payload;
using Expenso.Shared.Commands.Validation.Validators;
using Expenso.Shared.System.Serialization;
using Expenso.Shared.System.Serialization.Default.Settings;
using Expenso.Shared.System.Types.Clock;
using Expenso.Shared.System.Types.Messages.Interfaces;
using Expenso.Shared.Tests.Utils.UnitTests;
using Expenso.TimeManagement.Core.Application.JobEntries.Shared.BackgroundJobs.Events;
using Expenso.TimeManagement.Core.Application.JobEntries.Write.RegisterJobEntry;
using Expenso.TimeManagement.Core.Application.JobEntries.Write.RegisterJobEntry.DTO.Request.Validators;
using Expenso.TimeManagement.Core.Application.Shared.Settings;
using Expenso.TimeManagement.Shared.DTO.RegisterJobEntry.Request;

using Moq;

using NUnit.Framework;

namespace Expenso.TimeManagement.Tests.UnitTests.Application.JobEntries.Write.RegisterJob.
    RegisterJobEntryCommandValidator;

[TestFixture]
internal abstract class
    RegisterJobEntryCommandValidatorTestBase : TestBase<
    Core.Application.JobEntries.Write.RegisterJobEntry.RegisterJobEntryCommandValidator>
{
    [SetUp]
    public void SetUp()
    {
        BudgetPermissionRequestExpiredIntegrationEvent eventTrigger = new(MessageContext: null!,
            Payload: new BudgetPermissionRequestExpiredPayload(BudgetPermissionRequestId: Guid.NewGuid()));

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
            .Setup(expression: x => x.IsAllowable(AllowedEventType.BudgetPermissionRequestExpired))
            .Returns(value: true);

        _eventTypeResolver
            .Setup(expression: x => x.Resolve(AllowedEventType.BudgetPermissionRequestExpired))
            .Returns(value: typeof(BudgetPermissionRequestExpiredIntegrationEvent));

        IMessageContext messageContext = MessageContextFactoryMock.Object.Current();
        string eventData = _serializer.Object.Serialize(value: eventTrigger);

        RegisterJobEntryRequestJobEntryTrigger jobEntryTrigger = new(
            EventType: RegisterJobEntryRequestJobEntryTriggerAllowedEventType.BudgetPermissionRequestExpired,
            EventData: eventData);

        RegisterJobEntryRequest payload = new(MaxRetries: 5, JobEntryTriggers: [jobEntryTrigger], Interval: null,
            RunAt: _clockMock.Object.UtcNow);

        _registerJobEntryCommand = new RegisterJobEntryCommand(MessageContext: messageContext, Payload: payload);
        MessageContextValidator messageContextValidator = new();
        RegisterJobEntryRequestJobEntryPeriodIntervalValidator periodIntervalValidator = new();

        RegisterJobEntryRequestJobEntryTriggerValidator triggerValidator =
            new(serializer: _serializer.Object, eventTypeResolver: _eventTypeResolver.Object);

        RegisterJobEntryRequestValidator requestValidator = new(
            jobEntryPeriodIntervalValidator: periodIntervalValidator, jobEntryTriggerValidator: triggerValidator,
            clock: _clockMock.Object);

        TestCandidate = new Core.Application.JobEntries.Write.RegisterJobEntry.RegisterJobEntryCommandValidator(
            messageContextValidator: messageContextValidator, registerJobEntryRequestValidator: requestValidator);
    }

    [TearDown]
    public void TearDown()
    {
        _clockMock.Reset();
        _serializer.Reset();
        _eventTypeResolver.Reset();
        _clockMock = null!;
        _serializer = null!;
        _eventTypeResolver = null!;
        TestCandidate = null!;
    }

    protected Mock<IClock> _clockMock = null!;
    protected RegisterJobEntryCommand _registerJobEntryCommand = null!;
    protected Mock<IEventTypeResolver> _eventTypeResolver = null!;
    private Mock<ISerializer> _serializer = null!;
}