using Expenso.Shared.System.Modules.Constants;

using NUnit.Framework;

namespace Expenso.Api.Tests.E2E.UserPreferences.Preferences;

[TestFixture]
internal abstract class PreferencesTestBase : TestBase
{
    protected const string ApiRequestUrl = "user-preferences/preferences";
    
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
        AssertModuleHeader(response: response, moduleName: ModuleNames.UserPreferencesModule);
        base.AssertResponseOk(response: response);
    }

    protected override void AssertResponseCreated(HttpResponseMessage response)
    {
        AssertModuleHeader(response: response, moduleName: ModuleNames.UserPreferencesModule);
        base.AssertResponseCreated(response: response);
    }

    protected override void AssertResponseNoContent(HttpResponseMessage response)
    {
        AssertModuleHeader(response: response, moduleName: ModuleNames.UserPreferencesModule);
        base.AssertResponseNoContent(response: response);
    }
}