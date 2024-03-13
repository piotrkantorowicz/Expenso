using Expenso.Communication.Proxy.DTO.API.SendNotification;

using Moq;

namespace Expenso.Api.Tests.E2E.Communication;

internal sealed class SendNotificationAsync : CommunicationTestBase
{
    [Test]
    public void Should_SendNotification_And_NotThrow()
    {
        // Arrange
        SendNotificationRequest request = new("Subject", "Body",
            new SendNotificationRequest_NotificationContext("From", "To"),
            new SendNotificationRequest_NotificationType(true, true, true));

        // Act
        // Assert
        Assert.DoesNotThrowAsync(
            () => _communicationProxy.SendNotificationAsync(request, It.IsAny<CancellationToken>()));
    }
}