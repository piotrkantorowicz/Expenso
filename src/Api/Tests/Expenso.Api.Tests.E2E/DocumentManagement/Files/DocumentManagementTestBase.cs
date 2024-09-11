using Expenso.DocumentManagement.Proxy;

using Microsoft.Extensions.DependencyInjection;

namespace Expenso.Api.Tests.E2E.DocumentManagement.Files;

internal abstract class DocumentManagementTestBase : TestBase
{
    protected IDocumentManagementProxy _documentManagementProxy = null!;

    [SetUp]
    public override Task SetUpAsync()
    {
        _documentManagementProxy = WebApp.Instance.ServiceProvider.GetRequiredService<IDocumentManagementProxy>();

        return base.SetUpAsync();
    }

    [TearDown]
    public override Task TearDownAsync()
    {
        _documentManagementProxy = null!;

        return base.TearDownAsync();
    }
}