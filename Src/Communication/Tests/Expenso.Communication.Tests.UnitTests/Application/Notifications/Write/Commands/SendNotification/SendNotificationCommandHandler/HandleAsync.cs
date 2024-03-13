using Expenso.Communication.Core.Application.Notifications.Services.Emails;
using Expenso.Communication.Core.Application.Notifications.Services.InApp;
using Expenso.Communication.Core.Application.Notifications.Services.Push;
using Expenso.Communication.Core.Application.Notifications.Write.Commands.SendNotification;
using Expenso.Communication.Proxy.DTO.API.SendNotification;

using Moq;

namespace Expenso.Communication.Tests.UnitTests.Application.Notifications.Write.Commands.SendNotification.
    SendNotificationCommandHandler;

internal sealed class HandleAsync : SendNotificationCommandHandlerTestBase
{
    [Test]
    public async Task Should_SendEmail_When_NotificationTypeIsEmail()
    {
        // Arrange
        SendNotificationCommand command = new(MessageContextFactoryMock.Object.Current(),
            new SendNotificationRequest("Subject", "Content",
                new SendNotificationRequest_NotificationContext("From", "To"),
                new SendNotificationRequest_NotificationType(true, false, false)));

        // Act
        await TestCandidate.HandleAsync(command, CancellationToken.None);

        // Assert
        _notificationServiceFactoryMock.Verify(x => x.GetService<IEmailService>(), Times.Once);
    }

    [Test]
    public async Task Should_SendPush_When_NotificationTypeIsPush()
    {
        // Arrange
        SendNotificationCommand command = new(MessageContextFactoryMock.Object.Current(),
            new SendNotificationRequest("Subject", "Content",
                new SendNotificationRequest_NotificationContext("From", "To"),
                new SendNotificationRequest_NotificationType(false, true, false)));

        // Act
        await TestCandidate.HandleAsync(command, CancellationToken.None);

        // Assert
        _notificationServiceFactoryMock.Verify(x => x.GetService<IPushService>(), Times.Once);
    }

    [Test]
    public async Task Should_SendInApp_When_NotificationTypeIsInApp()
    {
        // Arrange
        SendNotificationCommand command = new(MessageContextFactoryMock.Object.Current(),
            new SendNotificationRequest("Subject", "Content",
                new SendNotificationRequest_NotificationContext("From", "To"),
                new SendNotificationRequest_NotificationType(false, false, true)));

        // Act
        await TestCandidate.HandleAsync(command, CancellationToken.None);

        // Assert
        _notificationServiceFactoryMock.Verify(x => x.GetService<IInAppService>(), Times.Once);
    }

    [Test]
    public async Task Should_SendAll_When_NotificationTypeIsAll()
    {
        // Arrange
        SendNotificationCommand command = new(MessageContextFactoryMock.Object.Current(),
            new SendNotificationRequest("Subject", "Content",
                new SendNotificationRequest_NotificationContext("From", "To"),
                new SendNotificationRequest_NotificationType(true, true, true)));

        // Act
        await TestCandidate.HandleAsync(command, CancellationToken.None);

        // Assert
        _notificationServiceFactoryMock.Verify(x => x.GetService<IEmailService>(), Times.Once);
        _notificationServiceFactoryMock.Verify(x => x.GetService<IPushService>(), Times.Once);
        _notificationServiceFactoryMock.Verify(x => x.GetService<IInAppService>(), Times.Once);
    }
}