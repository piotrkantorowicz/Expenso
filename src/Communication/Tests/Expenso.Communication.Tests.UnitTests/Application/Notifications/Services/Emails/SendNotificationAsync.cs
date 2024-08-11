using FluentAssertions;

namespace Expenso.Communication.Tests.UnitTests.Application.Notifications.Services.Emails;

internal sealed class SendNotificationAsync : FakeEmailServiceTestBase
{
    [Test]
    public void Should_SendEmail()
    {
        // Arrange
        const string from = "from";
        const string to = "to";
        const string replyTo = "replyTo";
        const string subject = "subject";
        const string content = "body";
        string[] cc = ["cc", "cc2"];
        string[] bcc = ["bcc", "bcc2", "bcc3"];

        //Act
        TestCandidate.SendNotificationAsync(from: from, to: to, subject: subject, content: content, cc: cc, bcc: bcc,
            replyTo: replyTo);

        //Assert
        _fakeLogger.Ex.Should().BeNull();

        _fakeLogger
            .Message.Should()
            .Be(expected:
                $"Email notification from {from} to {to} with cc {string.Join(separator: ",", value: cc)} and bcc {string.Join(separator: ",", value: bcc)} and replyTo {replyTo} with subject {subject} and content {content} sent successfully");
    }
}