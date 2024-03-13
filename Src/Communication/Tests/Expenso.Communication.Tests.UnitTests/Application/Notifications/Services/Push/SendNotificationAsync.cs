using FluentAssertions;

namespace Expenso.Communication.Tests.UnitTests.Application.Notifications.Services.Push;

internal sealed class SendNotificationAsync : FakePushServiceTestBase
{
    [Test]
    public void Should_SendPushNotification()
    {
        // Arrange
        const string from = "from";
        const string to = "to";
        const string subject = "subject";
        const string content = "body";

        //Act
        TestCandidate.SendNotificationAsync(from, to, subject, content);

        //Assert
        _fakeLogger.Ex.Should().BeNull();

        _fakeLogger
            .Message.Should()
            .Be(
                $"Push notification from {from} to {to} with subject {subject} and content {content} sent successfully");
    }
}