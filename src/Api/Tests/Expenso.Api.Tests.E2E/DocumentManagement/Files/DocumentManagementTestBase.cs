using Expenso.Api.Tests.E2E.Configuration;
using Expenso.DocumentManagement.Shared;

using Microsoft.Extensions.DependencyInjection;

using NUnit.Framework;

namespace Expenso.Api.Tests.E2E.DocumentManagement.Files;

[TestFixture]
internal abstract class DocumentManagementTestBase : TestBase
{
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

    protected IDocumentManagementProxy _documentManagementProxy = null!;
}