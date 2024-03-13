using Expenso.Communication.Core.Application.Notifications.Factories.Interfaces;
using Expenso.Communication.Core.Application.Notifications.Services.Emails;
using Expenso.Communication.Core.Application.Notifications.Services.InApp;
using Expenso.Communication.Core.Application.Notifications.Services.InApp.Acl.Fake;
using Expenso.Communication.Core.Application.Notifications.Services.Push;
using Expenso.Communication.Core.Application.Notifications.Services.Push.Acl.Fake;
using Expenso.Shared.Tests.Utils.UnitTests;

using Moq;

using TestCandidate =
    Expenso.Communication.Core.Application.Notifications.Write.Commands.SendNotification.SendNotificationCommandHandler;

namespace Expenso.Communication.Tests.UnitTests.Application.Notifications.Write.Commands.SendNotification.
    SendNotificationCommandHandler;

internal abstract class SendNotificationCommandHandlerTestBase : TestBase<TestCandidate>
{
    protected Mock<INotificationServiceFactory> _notificationServiceFactoryMock = null!;

    [SetUp]
    public void Setup()
    {
        _notificationServiceFactoryMock = new Mock<INotificationServiceFactory>();

        _notificationServiceFactoryMock
            .Setup(x => x.GetService<IEmailService>())
            .Returns(new Mock<IEmailService>().Object);

        _notificationServiceFactoryMock
            .Setup(x => x.GetService<IPushService>())
            .Returns(new Mock<IPushService>().Object);

        _notificationServiceFactoryMock
            .Setup(x => x.GetService<IInAppService>())
            .Returns(new Mock<IInAppService>().Object);

        TestCandidate = new TestCandidate(_notificationServiceFactoryMock.Object);
    }
}