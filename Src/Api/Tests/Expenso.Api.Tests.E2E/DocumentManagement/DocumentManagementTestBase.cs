using Expenso.DocumentManagement.Proxy;

using Microsoft.Extensions.DependencyInjection;

namespace Expenso.Api.Tests.E2E.DocumentManagement;

internal abstract class DocumentManagementTestBase : TestBase
{
    protected IDocumentManagementProxy _documentManagementProxy = null!;

    [SetUp]
    public override Task SetUp()
    {
        _documentManagementProxy = WebApp.Instance.ServiceProvider.GetRequiredService<IDocumentManagementProxy>();

        return base.SetUp();
    }

    [TearDown]
    public override Task TearDown()
    {
        _documentManagementProxy = null!;

        return base.TearDown();
    }
}