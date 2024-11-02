using Expenso.BudgetSharing.Domain.Shared.Shared.Notifications;
using Expenso.BudgetSharing.Domain.Shared.Shared.Notifications.Models;
using Expenso.BudgetSharing.Domain.Shared.ValueObjects;
using Expenso.Communication.Shared;
using Expenso.Communication.Shared.DTO.Settings;
using Expenso.Communication.Shared.DTO.Settings.Email;
using Expenso.Communication.Shared.DTO.Settings.InApp;
using Expenso.Communication.Shared.DTO.Settings.Push;
using Expenso.Shared.Domain.Events;
using Expenso.Shared.Domain.Types.Events;
using Expenso.Shared.System.Types.Clock;
using Expenso.Shared.Tests.Utils.UnitTests;

using Moq;

namespace Expenso.BudgetSharing.Tests.UnitTests.Domain.Shared.Base.DomainEventHandlers;

[TestFixture]
internal abstract class EventHandlerTestBase<T, TEvent> : TestBase<T> where T : class, IDomainEventHandler<TEvent>
    where TEvent : class, IDomainEvent
{
    [SetUp]
    public void SetUp()
    {
        _iIamProxyServiceMock = new Mock<IIamProxyService>();
        _communicationProxyMock = new Mock<ICommunicationProxy>();
        _clock = new Mock<IClock>();
        _clock.Setup(expression: x => x.UtcNow).Returns(value: DateTimeOffset.UtcNow);

        _notificationSettings = new NotificationSettings
        {
            Enabled = true,
            Email = new EmailNotificationSettings(Enabled: true, Smtp: null!, From: "SyedMandal@email.com",
                ReplyTo: "AbdoMo@email.com"),
            InApp = new InAppNotificationSettings(Enabled: true),
            Push = new PushNotificationSettings(Enabled: true)
        };

        _defaultOwnerId = PersonId.New(value: Guid.NewGuid());
        _defaultParticipantId = PersonId.New(value: Guid.NewGuid());

        _defaultNotificationRecipients = new NotificationRecipients(
            Owner: new NotificationRecipient(UserId: _defaultOwnerId.ToString(), Fullname: "Laura Ramirez",
                Email: "laura@email.com"), Participants:
            [
                new NotificationRecipient(UserId: _defaultParticipantId.ToString(), Email: "francisco224@email.com",
                    Fullname: "Francisco Ramirez"),
                new NotificationRecipient(UserId: Guid.NewGuid().ToString(), Email: "oliver12@email.com",
                    Fullname: "Oliver Cruz")
            ]);

        InitTestCandidate();
    }

    protected Mock<IClock> _clock = null!;
    protected Mock<ICommunicationProxy> _communicationProxyMock = null!;
    protected NotificationRecipients _defaultNotificationRecipients = null!;
    protected PersonId _defaultOwnerId = null!;
    protected PersonId _defaultParticipantId = null!;
    protected Mock<IIamProxyService> _iIamProxyServiceMock = null!;
    protected NotificationSettings _notificationSettings = null!;

    protected abstract void InitTestCandidate();
}