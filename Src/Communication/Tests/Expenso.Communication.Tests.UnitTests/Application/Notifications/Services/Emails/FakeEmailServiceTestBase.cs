using Expenso.Communication.Core.Application.Notifications.Services.Emails.Acl.Fake;
using Expenso.Shared.Tests.Utils.UnitTests;

namespace Expenso.Communication.Tests.UnitTests.Application.Notifications.Services.Emails;

internal abstract class FakeEmailServiceTestBase : TestBase<FakeEmailService>
{
    protected InMemoryFakeLogger<FakeEmailService> _fakeLogger = null!;

    [SetUp]
    public void SetUp()
    {
        _fakeLogger = new InMemoryFakeLogger<FakeEmailService>();
        TestCandidate = new FakeEmailService(_fakeLogger);
    }
}