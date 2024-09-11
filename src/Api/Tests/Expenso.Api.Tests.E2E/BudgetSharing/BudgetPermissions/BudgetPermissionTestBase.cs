namespace Expenso.Api.Tests.E2E.BudgetSharing.BudgetPermissions;

internal abstract class BudgetPermissionTestBase : TestBase
{
    [SetUp]
    public override Task SetUp()
    {
        return base.SetUp();
    }

    [TearDown]
    public override Task TearDown()
    {
        return base.TearDown();
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