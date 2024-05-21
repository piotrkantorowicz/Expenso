using Expenso.Shared.System.Types.Clock;
using Expenso.TimeManagement.Proxy;

using Microsoft.Extensions.DependencyInjection;

using Moq;

namespace Expenso.Api.Tests.E2E.TimeManagement.JobEntries;

internal abstract class JobEntriesTestBase : TestBase
{
    protected ITimeManagementProxy _timeManagementProxy = null!;
    protected Mock<IClock> _clockMock = null!;

    [SetUp]
    public override Task SetUp()
    {
        _timeManagementProxy = WebApp.Instance.ServiceProvider.GetRequiredService<ITimeManagementProxy>();
        _clockMock = new Mock<IClock>();
        _clockMock.Setup(x => x.UtcNow).Returns(DateTimeOffset.UtcNow.AddMilliseconds(500));

        return base.SetUp();
    }

    [TearDown]
    public override Task TearDown()
    {
        _timeManagementProxy = null!;
        _clockMock.Reset();
        _clockMock = null!;

        return base.TearDown();
    }
}