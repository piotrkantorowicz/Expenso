using System.Reflection;

using Expenso.DocumentManagement.Core;
using Expenso.DocumentManagement.Proxy;
using Expenso.Shared.System.Modules;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using CoreExtensions = Expenso.DocumentManagement.Core.Extensions;

namespace Expenso.DocumentManagement.Api;

public sealed class DocumentManagementModule : ModuleDefinition
{
    public override string ModulePrefix => "/document-management";

    public override IReadOnlyCollection<Assembly> GetModuleAssemblies()
    {
        return
        [
            typeof(DocumentManagementModule).Assembly,
            typeof(CoreExtensions).Assembly,
            typeof(IDocumentManagementProxy).Assembly
        ];
    }

    public override void AddDependencies(IServiceCollection services, IConfiguration configuration)
    {
        services.AddDocumentManagementCore(configuration: configuration);
        services.AddDocumentManagementProxy(assemblies: GetModuleAssemblies());
    }

    public override IReadOnlyCollection<EndpointRegistration> CreateEndpoints()
    {
        return Array.Empty<EndpointRegistration>();
    }
}