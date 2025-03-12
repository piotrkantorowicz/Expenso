using System.Net;

using Expenso.Shared.System.Modules.Constants;

using NUnit.Framework;

namespace Expenso.Api.Tests.E2E.BudgetSharing.BudgetPermissionRequests;

[TestFixture]
internal abstract class BudgetPermissionRequestTestBase : TestBase
{
    protected const string ApiRequestUrl = "budget-sharing/budget-permission-requests";

    [SetUp]
    public override Task SetUpAsync()
    {
        return base.SetUpAsync();
    }

    [TearDown]
    public override Task TearDownAsync()
    {
        return base.TearDownAsync();
    }

    protected static void AssertResponse(HttpResponseMessage response, HttpStatusCode statusCode)
    {
        AssertResponseStatusCode(response: response, statusCode: statusCode);
        AssertCorrelationIdHeader(response: response);
        AssertModuleIdHeader(response: response, moduleName: ModuleNames.BudgetSharingModule);
        AssertTimezoneIdHeader(response: response);
    }
}