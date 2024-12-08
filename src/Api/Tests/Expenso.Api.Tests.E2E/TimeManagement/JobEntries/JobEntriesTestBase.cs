using Expenso.Shared.System.Modules.Constants;
using Expenso.Shared.System.Types.Clock;
using Expenso.TimeManagement.Shared;

using Microsoft.Extensions.DependencyInjection;

namespace Expenso.Api.Tests.E2E.TimeManagement.JobEntries;

[TestFixture]
internal abstract class JobEntriesTestBase : TestBase
{
    [SetUp]
    public override Task SetUpAsync()
    {
        _timeManagementProxy = WebApp.Instance.ServiceProvider.GetRequiredService<ITimeManagementProxy>();
        _clock = WebApp.Instance.ServiceProvider.GetRequiredService<IClock>();

        return base.SetUpAsync();
    }

    [TearDown]
    public override Task TearDownAsync()
    {
        _timeManagementProxy = null!;
        _clock = null!;

        return base.TearDownAsync();
    }

    protected IClock _clock = null!;
    protected ITimeManagementProxy _timeManagementProxy = null!;

    protected override void AssertResponseOk(HttpResponseMessage response)
    {
        AssertModuleHeader(response: response, moduleName: ModuleNames.TimeManagementModule);
        base.AssertResponseOk(response: response);
    }

    protected override void AssertResponseCreated(HttpResponseMessage response)
    {
        AssertModuleHeader(response: response, moduleName: ModuleNames.TimeManagementModule);
        base.AssertResponseCreated(response: response);
    }

    protected override void AssertResponseNoContent(HttpResponseMessage response)
    {
        AssertModuleHeader(response: response, moduleName: ModuleNames.TimeManagementModule);
        base.AssertResponseNoContent(response: response);
    }
}