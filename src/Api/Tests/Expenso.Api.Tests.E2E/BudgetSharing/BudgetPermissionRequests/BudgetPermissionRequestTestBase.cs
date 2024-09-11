namespace Expenso.Api.Tests.E2E.BudgetSharing.BudgetPermissionRequests;

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
        AssertModuleHeader(response: response, moduleName: "BudgetSharingModule");
        base.AssertResponseOk(response: response);
    }

    protected override void AssertResponseCreated(HttpResponseMessage response)
    {
        AssertModuleHeader(response: response, moduleName: "BudgetSharingModule");
        base.AssertResponseCreated(response: response);
    }

    protected override void AssertResponseNoContent(HttpResponseMessage response)
    {
        AssertModuleHeader(response: response, moduleName: "BudgetSharingModule");
        base.AssertResponseNoContent(response: response);
    }
}