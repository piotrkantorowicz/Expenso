using Expenso.Communication.Proxy;

using Microsoft.Extensions.DependencyInjection;

namespace Expenso.Api.Tests.E2E.Communication;

internal abstract class CommunicationTestBase : TestBase
{
    protected ICommunicationProxy _communicationProxy = null!;

    [SetUp]
    public override Task SetUpAsync()
    {
        _communicationProxy = WebApp.Instance.ServiceProvider.GetRequiredService<ICommunicationProxy>();

        return base.SetUpAsync();
    }

    [TearDown]
    public override Task TearDownAsync()
    {
        _communicationProxy = null!;

        return base.TearDownAsync();
    }
}