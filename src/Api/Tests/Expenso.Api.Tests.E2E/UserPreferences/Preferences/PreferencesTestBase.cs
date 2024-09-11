namespace Expenso.Api.Tests.E2E.UserPreferences.Preferences;

internal abstract class PreferencesTestBase : TestBase
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
        AssertModuleHeader(response: response, moduleName: "UserPreferencesModule");
        base.AssertResponseOk(response: response);
    }

    protected override void AssertResponseCreated(HttpResponseMessage response)
    {
        AssertModuleHeader(response: response, moduleName: "UserPreferencesModule");
        base.AssertResponseCreated(response: response);
    }

    protected override void AssertResponseNoContent(HttpResponseMessage response)
    {
        AssertModuleHeader(response: response, moduleName: "UserPreferencesModule");
        base.AssertResponseNoContent(response: response);
    }
}