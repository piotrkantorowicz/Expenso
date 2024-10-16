using Expenso.Communication.Core.Application.Notifications.Services.Push.Acl.Fake;
using Expenso.Shared.System.Logging;
using Expenso.Shared.Tests.Utils.UnitTests;

using Moq;

namespace Expenso.Communication.Tests.UnitTests.Application.Notifications.Services.Push;

[TestFixture]
internal abstract class FakePushServiceTestBase : TestBase<FakePushService>
{
    [SetUp]
    public void SetUp()
    {
        _loggerServiceMock = new Mock<ILoggerService<FakePushService>>();
        TestCandidate = new FakePushService(logger: _loggerServiceMock.Object);
    }

    protected Mock<ILoggerService<FakePushService>> _loggerServiceMock = null!;
}