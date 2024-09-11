using Expenso.Shared.System.Types.Clock;
using Expenso.TimeManagement.Proxy;

using Microsoft.Extensions.DependencyInjection;

using Moq;

namespace Expenso.Api.Tests.E2E.TimeManagement.JobEntries;

internal abstract class JobEntriesTestBase : TestBase
{
    protected Mock<IClock> _clockMock = null!;
    protected ITimeManagementProxy _timeManagementProxy = null!;

    [SetUp]
    public override Task SetUp()
    {
        _timeManagementProxy = WebApp.Instance.ServiceProvider.GetRequiredService<ITimeManagementProxy>();
        _clockMock = new Mock<IClock>();

        _clockMock
            .Setup(expression: x => x.UtcNow)
            .Returns(value: DateTimeOffset.UtcNow.AddMilliseconds(milliseconds: 500));

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

    protected override void AssertResponseOk(HttpResponseMessage response)
    {
        AssertModuleHeader(response: response, moduleName: "TimeManagementModule");
        base.AssertResponseOk(response: response);
    }

    protected override void AssertResponseCreated(HttpResponseMessage response)
    {
        AssertModuleHeader(response: response, moduleName: "TimeManagementModule");
        base.AssertResponseCreated(response: response);
    }

    protected override void AssertResponseNoContent(HttpResponseMessage response)
    {
        AssertModuleHeader(response: response, moduleName: "TimeManagementModule");
        base.AssertResponseNoContent(response: response);
    }
}