using Expenso.Communication.Shared;

using Microsoft.Extensions.DependencyInjection;

namespace Expenso.Api.Tests.E2E.Communication;

[TestFixture]
internal abstract class CommunicationTestBase : TestBase
{
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

    protected ICommunicationProxy _communicationProxy = null!;
}