using Expenso.Shared.System.Logging;
using Expenso.Shared.System.Types.Messages.Interfaces;

namespace Expenso.Communication.Tests.UnitTests.Application.Notifications.Services.Emails;

[TestFixture]
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
        IMessageContext messageContext = MessageContextFactoryMock.Object.Current();

        //Act
        TestCandidate.SendNotificationAsync(messageContext: messageContext, from: from, to: to, subject: subject,
            content: content, cc: cc, bcc: bcc, replyTo: replyTo);

        //Assert
        _loggerServiceMock.Verify(expression: x => x.LogInfo(LoggingUtils.GeneralInformation,
            "Email notification from {From} to {To} with cc {Cc} and bcc {Bcc} and replyTo {ReplyTo} with subject {Subject} and content {Content} sent successfully",
            messageContext, from, to, string.Join(",", cc), string.Join(",", bcc), replyTo, subject, content));
    }
}