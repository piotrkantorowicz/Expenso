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

    protected override void AssertResponseOk(HttpResponseMessage response)
    {
        AssertModuleHeader(response: response, moduleName: ModuleNames.BudgetSharingModule);
        base.AssertResponseOk(response: response);
    }

    protected override void AssertResponseCreated(HttpResponseMessage response)
    {
        AssertModuleHeader(response: response, moduleName: ModuleNames.BudgetSharingModule);
        base.AssertResponseCreated(response: response);
    }

    protected override void AssertResponseNoContent(HttpResponseMessage response)
    {
        AssertModuleHeader(response: response, moduleName: ModuleNames.BudgetSharingModule);
        base.AssertResponseNoContent(response: response);
    }

    protected override void AssertResponseBadRequest(HttpResponseMessage response)
    {
        AssertModuleHeader(response: response, moduleName: ModuleNames.BudgetSharingModule);
        base.AssertResponseBadRequest(response: response);
    }
}