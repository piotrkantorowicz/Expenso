using Expenso.Communication.Core.Application.Notifications.Services.InApp.Acl.Fake;
using Expenso.Shared.Tests.Utils.UnitTests;

namespace Expenso.Communication.Tests.UnitTests.Application.Notifications.Services.InApp;

internal abstract class FakeInAppServiceTestBase : TestBase<FakeInAppService>
{
    protected InMemoryFakeLogger<FakeInAppService> _fakeLogger = null!;

    [SetUp]
    public void SetUp()
    {
        _fakeLogger = new InMemoryFakeLogger<FakeInAppService>();
        TestCandidate = new FakeInAppService(logger: _fakeLogger);
    }
}