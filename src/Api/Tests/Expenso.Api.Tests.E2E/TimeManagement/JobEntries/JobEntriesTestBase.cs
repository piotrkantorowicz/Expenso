using Expenso.Shared.System.Types.Clock;
using Expenso.TimeManagement.Shared;

using Microsoft.Extensions.DependencyInjection;

using Moq;

namespace Expenso.Api.Tests.E2E.TimeManagement.JobEntries;

[TestFixture]
internal abstract class JobEntriesTestBase : TestBase
{
    [SetUp]
    public override Task SetUpAsync()
    {
        _timeManagementProxy = WebApp.Instance.ServiceProvider.GetRequiredService<ITimeManagementProxy>();
        _clockMock = new Mock<IClock>();

        _clockMock
            .Setup(expression: x => x.UtcNow)
            .Returns(value: DateTimeOffset.UtcNow.AddMilliseconds(milliseconds: 500));

        return base.SetUpAsync();
    }

    [TearDown]
    public override Task TearDownAsync()
    {
        _timeManagementProxy = null!;
        _clockMock.Reset();
        _clockMock = null!;

        return base.TearDownAsync();
    }

    protected Mock<IClock> _clockMock = null!;
    protected ITimeManagementProxy _timeManagementProxy = null!;

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