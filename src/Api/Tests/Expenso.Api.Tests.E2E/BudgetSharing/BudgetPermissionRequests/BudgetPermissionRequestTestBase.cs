using Expenso.Shared.System.Modules.Constants;

using NUnit.Framework;

namespace Expenso.Api.Tests.E2E.BudgetSharing.BudgetPermissionRequests;

[TestFixture]
internal abstract class BudgetPermissionRequestTestBase : TestBase
{
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
}