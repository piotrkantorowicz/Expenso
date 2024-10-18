using Expenso.Communication.Core.Application.Notifications.Write.Commands.SendNotification.DTO.Validators;
using Expenso.Shared.Commands.Validation.Validators;
using Expenso.Shared.Tests.Utils.UnitTests;

namespace Expenso.Communication.Tests.UnitTests.Application.Notifications.Write.Commands.SendNotification.
    SendNotificationCommandValidator;

[TestFixture]
internal abstract class SendNotificationCommandValidatorTestBase : TestBase<
    Core.Application.Notifications.Write.Commands.SendNotification.SendNotificationCommandValidator>
{
    [SetUp]
    public void Setup()
    {
        SendNotificationRequest_NotificationTypeValidator notificationTypeValidator = new();
        SendNotificationRequest_NotificationContextValidator notificationContextValidator = new();

        SendNotificationRequestValidator sendNotificationRequestValidator =
            new(sendNotificationRequestNotificationContextValidator: notificationContextValidator,
                sendNotificationRequestNotificationTypeValidator: notificationTypeValidator);

        MessageContextValidator messageContextValidator = new();

        TestCandidate =
            new Core.Application.Notifications.Write.Commands.SendNotification.SendNotificationCommandValidator(
                messageContextValidator: messageContextValidator,
                sendNotificationRequestValidator: sendNotificationRequestValidator);
    }
}