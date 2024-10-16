using Expenso.Communication.Core.Application.Notifications.Services.Emails.Acl.Fake;
using Expenso.Shared.System.Logging;
using Expenso.Shared.Tests.Utils.UnitTests;

using Moq;

namespace Expenso.Communication.Tests.UnitTests.Application.Notifications.Services.Emails;

[TestFixture]
internal abstract class FakeEmailServiceTestBase : TestBase<FakeEmailService>
{
    [SetUp]
    public void SetUp()
    {
        _loggerServiceMock = new Mock<ILoggerService<FakeEmailService>>();
        TestCandidate = new FakeEmailService(logger: _loggerServiceMock.Object);
    }

    protected Mock<ILoggerService<FakeEmailService>> _loggerServiceMock = null!;
}