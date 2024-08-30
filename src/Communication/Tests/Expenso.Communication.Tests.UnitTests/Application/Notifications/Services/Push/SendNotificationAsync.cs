using Expenso.Shared.System.Logging;
using Expenso.Shared.System.Types.Messages.Interfaces;

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
        IMessageContext messageContext = MessageContextFactoryMock.Object.Current();

        //Act
        TestCandidate.SendNotificationAsync(messageContext: messageContext, from: from, to: to, subject: subject,
            content: content);

        //Assert
        _loggerServiceMock.Verify(expression: x => x.LogInfo(LoggingUtils.GeneralInformation,
            "Push notification from {From} to {To} with subject {Subject} and content {Content} sent successfully",
            messageContext, from, to, subject, content));
    }
}