using System.Net;

using Expenso.Api.Tests.E2E.Configuration;
using Expenso.Shared.System.Modules.Constants;
using Expenso.Shared.System.Time;
using Expenso.TimeManagement.Shared;

using Microsoft.Extensions.DependencyInjection;

using NUnit.Framework;

namespace Expenso.Api.Tests.E2E.TimeManagement.JobEntries;

[TestFixture]
internal abstract class JobEntriesTestBase : TestBase
{
    protected const string ApiRequestUrl = "time-management/job-entries";
    
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

    protected static void AssertResponse(HttpResponseMessage response, HttpStatusCode statusCode)
    {
        AssertResponseStatusCode(response: response, statusCode: statusCode);
        AssertCorrelationIdHeader(response: response);
        AssertModuleIdHeader(response: response, moduleName: ModuleNames.TimeManagementModule);
        AssertTimezoneIdHeader(response: response);
    }
}