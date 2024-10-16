using Expenso.Communication.Core.Application.Notifications.Services.InApp.Acl.Fake;
using Expenso.Shared.System.Logging;
using Expenso.Shared.Tests.Utils.UnitTests;

using Moq;

namespace Expenso.Communication.Tests.UnitTests.Application.Notifications.Services.InApp;

[TestFixture]
internal abstract class FakeInAppServiceTestBase : TestBase<FakeInAppService>
{
    [SetUp]
    public void SetUp()
    {
        _loggerServiceMock = new Mock<ILoggerService<FakeInAppService>>();
        TestCandidate = new FakeInAppService(logger: _loggerServiceMock.Object);
    }

    protected Mock<ILoggerService<FakeInAppService>> _loggerServiceMock = null!;
}