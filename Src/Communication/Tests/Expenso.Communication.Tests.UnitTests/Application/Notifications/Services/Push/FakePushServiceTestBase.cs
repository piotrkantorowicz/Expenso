using Expenso.Communication.Core.Application.Notifications.Services.Push.Acl.Fake;
using Expenso.Shared.Tests.Utils.UnitTests;

namespace Expenso.Communication.Tests.UnitTests.Application.Notifications.Services.Push;

internal abstract class FakePushServiceTestBase : TestBase<FakePushService>
{
    protected InMemoryFakeLogger<FakePushService> _fakeLogger = null!;

    [SetUp]
    public void SetUp()
    {
        _fakeLogger = new InMemoryFakeLogger<FakePushService>();
        TestCandidate = new FakePushService(_fakeLogger);
    }
}