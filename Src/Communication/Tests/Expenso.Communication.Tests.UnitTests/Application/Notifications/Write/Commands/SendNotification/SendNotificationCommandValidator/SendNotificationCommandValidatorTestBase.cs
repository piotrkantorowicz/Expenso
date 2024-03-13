using Expenso.Shared.Tests.Utils.UnitTests;

using TestCandidate =
    Expenso.Communication.Core.Application.Notifications.Write.Commands.SendNotification.
    SendNotificationCommandValidator;

namespace Expenso.Communication.Tests.UnitTests.Application.Notifications.Write.Commands.SendNotification.
    SendNotificationCommandValidator;

internal abstract class SendNotificationCommandValidatorTestBase : TestBase<TestCandidate>
{
    [SetUp]
    public void Setup()
    {
        TestCandidate = new TestCandidate();
    }
}