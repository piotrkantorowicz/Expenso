using Expenso.Api.Tests.E2E.TestData;
using Expenso.UserPreferences.Proxy;

using Microsoft.Extensions.DependencyInjection;

namespace Expenso.Api.Tests.E2E;

[SetUpFixture]
internal sealed class WebAppTestSetup
{
    [OneTimeSetUp]
    public async Task OneTimeSetup()
    {
        using IServiceScope scope = WebApp.Instance.ServiceProvider.CreateScope();
        IUserPreferencesProxy preferencesProxy = scope.ServiceProvider.GetRequiredService<IUserPreferencesProxy>();
        await PreferencesDataProvider.Initialize(preferencesProxy, default);
    }

    [OneTimeTearDown]
    public void OneTimeTearDown()
    {
        WebApp.Instance.Destroy();
    }
}