using Expenso.Communication.Core.Application.Notifications.Services.InApp.Acl.Fake;
using Expenso.Shared.System.Logging;
using Expenso.Shared.Tests.Utils.UnitTests;

using Moq;

namespace Expenso.Communication.Tests.UnitTests.Application.Notifications.Services.InApp;

internal abstract class FakeInAppServiceTestBase : TestBase<FakeInAppService>
{
    protected Mock<ILoggerService<FakeInAppService>> _loggerServiceMock = null!;

    [SetUp]
    public void SetUp()
    {
        _loggerServiceMock = new Mock<ILoggerService<FakeInAppService>>();
        TestCandidate = new FakeInAppService(logger: _loggerServiceMock.Object);
    }
}