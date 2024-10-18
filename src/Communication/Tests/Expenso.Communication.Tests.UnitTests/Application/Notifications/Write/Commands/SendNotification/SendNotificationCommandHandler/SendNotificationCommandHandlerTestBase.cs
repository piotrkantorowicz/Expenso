using Expenso.Communication.Core.Application.Notifications.Factories.Interfaces;
using Expenso.Communication.Core.Application.Notifications.Services.Emails;
using Expenso.Communication.Core.Application.Notifications.Services.InApp;
using Expenso.Communication.Core.Application.Notifications.Services.Push;
using Expenso.Shared.Tests.Utils.UnitTests;

using Moq;

namespace Expenso.Communication.Tests.UnitTests.Application.Notifications.Write.Commands.SendNotification.
    SendNotificationCommandHandler;

[TestFixture]
internal abstract class SendNotificationCommandHandlerTestBase : TestBase<
    Core.Application.Notifications.Write.Commands.SendNotification.SendNotificationCommandHandler>
{
    [SetUp]
    public void Setup()
    {
        _notificationServiceFactoryMock = new Mock<INotificationServiceFactory>();

        _notificationServiceFactoryMock
            .Setup(expression: x => x.GetService<IEmailService>())
            .Returns(value: new Mock<IEmailService>().Object);

        _notificationServiceFactoryMock
            .Setup(expression: x => x.GetService<IPushService>())
            .Returns(value: new Mock<IPushService>().Object);

        _notificationServiceFactoryMock
            .Setup(expression: x => x.GetService<IInAppService>())
            .Returns(value: new Mock<IInAppService>().Object);

        TestCandidate =
            new Core.Application.Notifications.Write.Commands.SendNotification.SendNotificationCommandHandler(
                notificationServiceFactory: _notificationServiceFactoryMock.Object);
    }

    protected Mock<INotificationServiceFactory> _notificationServiceFactoryMock = null!;
}