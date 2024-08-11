using Expenso.Communication.Proxy;

using Microsoft.Extensions.DependencyInjection;

namespace Expenso.Api.Tests.E2E.Communication;

internal abstract class CommunicationTestBase : TestBase
{
    protected ICommunicationProxy _communicationProxy = null!;

    [SetUp]
    public override Task SetUp()
    {
        _communicationProxy = WebApp.Instance.ServiceProvider.GetRequiredService<ICommunicationProxy>();

        return base.SetUp();
    }

    [TearDown]
    public override Task TearDown()
    {
        _communicationProxy = null!;

        return base.TearDown();
    }
}