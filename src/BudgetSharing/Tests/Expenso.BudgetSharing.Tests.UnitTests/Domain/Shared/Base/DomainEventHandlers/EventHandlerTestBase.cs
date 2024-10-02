using Expenso.BudgetSharing.Domain.Shared.Shared.Notifications;
using Expenso.BudgetSharing.Domain.Shared.Shared.Notifications.Models;
using Expenso.BudgetSharing.Domain.Shared.ValueObjects;
using Expenso.Communication.Proxy;
using Expenso.Communication.Proxy.DTO.Settings;
using Expenso.Communication.Proxy.DTO.Settings.Email;
using Expenso.Communication.Proxy.DTO.Settings.InApp;
using Expenso.Communication.Proxy.DTO.Settings.Push;
using Expenso.IAM.Proxy.DTO.GetUser;
using Expenso.Shared.Domain.Events;
using Expenso.Shared.Domain.Types.Events;
using Expenso.Shared.System.Types.Clock;
using Expenso.Shared.Tests.Utils.UnitTests;

using Moq;

namespace Expenso.BudgetSharing.Tests.UnitTests.Domain.Shared.Base.DomainEventHandlers;

internal abstract class EventHandlerTestBase<T, TEvent> : TestBase<T> where T : class, IDomainEventHandler<TEvent>
    where TEvent : class, IDomainEvent
{
    protected Mock<IClock> _clock = null!;
    protected Mock<ICommunicationProxy> _communicationProxyMock = null!;
    protected UserNotificationModel _defaultNotificationModel = null!;
    protected PersonId _defaultOwnerId = null!;
    protected PersonId _defaultParticipantId = null!;
    protected Mock<IIamProxyService> _iIamProxyServiceMock = null!;
    protected NotificationSettings _notificationSettings = null!;

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

        _defaultNotificationModel = new UserNotificationModel(
            Owner: new PersonNotificationModel(
                Person: new GetUserResponse(UserId: _defaultOwnerId.ToString(), Firstname: "Laura", Lastname: "Ramirez",
                    Username: "laur123", Email: "laura@email.com"), CanSendNotification: true), Participants:
            [
                new PersonNotificationModel(
                    Person: new GetUserResponse(UserId: _defaultParticipantId.ToString(), Firstname: "Francisco",
                        Lastname: "Yue", Username: "francisco224", Email: "francisco224@email.com"),
                    CanSendNotification: true)
            ]);

        InitTestCandidate();
    }

    protected abstract void InitTestCandidate();
}