using Expenso.Communication.Core.Application.Notifications.Services.Emails;
using Expenso.Communication.Core.Application.Notifications.Services.InApp;
using Expenso.Communication.Core.Application.Notifications.Services.Push;
using Expenso.Communication.Core.Application.Notifications.Write.Commands.SendNotification;
using Expenso.Communication.Shared.DTO.API.SendNotification;

using Moq;

namespace Expenso.Communication.Tests.UnitTests.Application.Notifications.Write.Commands.SendNotification.
    SendNotificationCommandHandler;

[TestFixture]
internal sealed class HandleAsync : SendNotificationCommandHandlerTestBase
{
    [Test]
    public async Task Should_SendEmail_When_NotificationTypeIsEmail()
    {
        // Arrange
        SendNotificationCommand command = new(MessageContext: MessageContextFactoryMock.Object.Current(),
            SendNotificationRequest: new SendNotificationRequest(Subject: "Subject", Content: "Content",
                NotificationContext: new SendNotificationRequest_NotificationContext(From: "From", To: "To"),
                NotificationType: new SendNotificationRequest_NotificationType(Email: true, Push: false,
                    InApp: false)));

        // Act
        await TestCandidate.HandleAsync(command: command, cancellationToken: CancellationToken.None);

        // Assert
        _notificationServiceFactoryMock.Verify(expression: x => x.GetService<IEmailService>(), times: Times.Once);
    }

    [Test]
    public async Task Should_SendPush_When_NotificationTypeIsPush()
    {
        // Arrange
        SendNotificationCommand command = new(MessageContext: MessageContextFactoryMock.Object.Current(),
            SendNotificationRequest: new SendNotificationRequest(Subject: "Subject", Content: "Content",
                NotificationContext: new SendNotificationRequest_NotificationContext(From: "From", To: "To"),
                NotificationType: new SendNotificationRequest_NotificationType(Email: false, Push: true,
                    InApp: false)));

        // Act
        await TestCandidate.HandleAsync(command: command, cancellationToken: CancellationToken.None);

        // Assert
        _notificationServiceFactoryMock.Verify(expression: x => x.GetService<IPushService>(), times: Times.Once);
    }

    [Test]
    public async Task Should_SendInApp_When_NotificationTypeIsInApp()
    {
        // Arrange
        SendNotificationCommand command = new(MessageContext: MessageContextFactoryMock.Object.Current(),
            SendNotificationRequest: new SendNotificationRequest(Subject: "Subject", Content: "Content",
                NotificationContext: new SendNotificationRequest_NotificationContext(From: "From", To: "To"),
                NotificationType: new SendNotificationRequest_NotificationType(Email: false, Push: false,
                    InApp: true)));

        // Act
        await TestCandidate.HandleAsync(command: command, cancellationToken: CancellationToken.None);

        // Assert
        _notificationServiceFactoryMock.Verify(expression: x => x.GetService<IInAppService>(), times: Times.Once);
    }

    [Test]
    public async Task Should_SendAll_When_NotificationTypeIsAll()
    {
        // Arrange
        SendNotificationCommand command = new(MessageContext: MessageContextFactoryMock.Object.Current(),
            SendNotificationRequest: new SendNotificationRequest(Subject: "Subject", Content: "Content",
                NotificationContext: new SendNotificationRequest_NotificationContext(From: "From", To: "To"),
                NotificationType: new SendNotificationRequest_NotificationType(Email: true, Push: true, InApp: true)));

        // Act
        await TestCandidate.HandleAsync(command: command, cancellationToken: CancellationToken.None);

        // Assert
        _notificationServiceFactoryMock.Verify(expression: x => x.GetService<IEmailService>(), times: Times.Once);
        _notificationServiceFactoryMock.Verify(expression: x => x.GetService<IPushService>(), times: Times.Once);
        _notificationServiceFactoryMock.Verify(expression: x => x.GetService<IInAppService>(), times: Times.Once);
    }
}