using Expenso.Api.Tests.E2E.Configuration;
using Expenso.Api.Tests.E2E.TestData;
using Expenso.Communication.Shared.DTO.API.SendNotification;
using Expenso.Shared.System.Modules.Constants;
using Expenso.Shared.System.Time;
using Expenso.Shared.System.Types.Messages;

using FluentAssertions;

using Microsoft.Extensions.DependencyInjection;

using Moq;

using NUnit.Framework;

namespace Expenso.Api.Tests.E2E.Communication;

[TestFixture]
internal sealed class SendNotificationAsync : CommunicationTestBase
{
    [Test]
    public async Task Should_SendNotification_And_NotThrow()
    {
        // Arrange
        IClock clock = WebApp.Instance.ServiceProvider.GetRequiredService<IClock>();

        SendNotificationRequest request = new(Subject: "Subject", Content: "Body",
            NotificationContext: new SendNotificationRequestNotificationContext(From: "From", To: "To"),
            NotificationType: new SendNotificationRequestNotificationType(Email: true, Push: true, InApp: true));

        // Act
        Func<Task> action = () => _communicationProxy.SendNotificationAsync(request: request,
            messageContext: new MessageContext(messageId: Guid.NewGuid(), correlationId: Guid.NewGuid(),
                requestedBy: TestClient.ClientId, timestamp: clock.UtcNow, module: ModuleNames.CommunicationModule),
            cancellationToken: It.IsAny<CancellationToken>());

        // Assert
        await action.Should().NotThrowAsync();
    }
}