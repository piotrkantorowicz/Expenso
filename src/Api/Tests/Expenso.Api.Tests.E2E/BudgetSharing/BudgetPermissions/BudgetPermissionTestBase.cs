using System.Net;

using Expenso.Shared.System.Modules.Constants;

using NUnit.Framework;

namespace Expenso.Api.Tests.E2E.BudgetSharing.BudgetPermissions;

[TestFixture]
internal abstract class BudgetPermissionTestBase : TestBase
{
    protected const string ApiRequestUrl = "budget-sharing/budget-permissions";
    
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