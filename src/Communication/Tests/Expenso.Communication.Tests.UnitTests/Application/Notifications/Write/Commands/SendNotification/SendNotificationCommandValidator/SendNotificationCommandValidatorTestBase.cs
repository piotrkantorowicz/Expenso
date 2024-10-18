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
        TestCandidate =
            new Core.Application.Notifications.Write.Commands.SendNotification.SendNotificationCommandValidator();
    }
}