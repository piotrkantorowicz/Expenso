using System.Net;

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

    protected static void AssertResponse(HttpResponseMessage response, HttpStatusCode statusCode)
    {
        AssertResponseStatusCode(response: response, statusCode: statusCode);
        AssertCorrelationIdHeader(response: response);
        AssertModuleIdHeader(response: response, moduleName: ModuleNames.UserPreferencesModule);
        AssertTimezoneIdHeader(response: response);
    }
}