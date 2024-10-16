using Expenso.Communication.Shared.DTO.API.SendNotification;

using Moq;

namespace Expenso.Api.Tests.E2E.Communication;

internal sealed class SendNotificationAsync : CommunicationTestBase
{
    [Test]
    public void Should_SendNotification_And_NotThrow()
    {
        // Arrange
        SendNotificationRequest request = new(Subject: "Subject", Content: "Body",
            NotificationContext: new SendNotificationRequest_NotificationContext(From: "From", To: "To"),
            NotificationType: new SendNotificationRequest_NotificationType(Email: true, Push: true, InApp: true));

        // Act
        // Assert
        Assert.DoesNotThrowAsync(code: () =>
            _communicationProxy.SendNotificationAsync(request: request,
                cancellationToken: It.IsAny<CancellationToken>()));
    }
}