using System.Reflection;

using Expenso.DocumentManagement.Core;
using Expenso.DocumentManagement.Proxy;
using Expenso.Shared.System.Modules;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using CoreServiceCollectionExtensions = Expenso.DocumentManagement.Core.ServiceCollectionExtensions;

namespace Expenso.DocumentManagement.Api;

public sealed class DocumentManagementModule : ModuleDefinition
{
    public override string ModulePrefix => "/document-management";

    public override IReadOnlyCollection<Assembly> GetModuleAssemblies()
    {
        return
        [
            typeof(DocumentManagementModule).Assembly,
            typeof(CoreServiceCollectionExtensions).Assembly,
            typeof(IDocumentManagementProxy).Assembly
        ];
    }

    public override void AddDependencies(IServiceCollection services, IConfiguration configuration)
    {
        services.AddDocumentManagementCore(configuration);
    }

    public override IReadOnlyCollection<EndpointRegistration> CreateEndpoints()
    {
        return Array.Empty<EndpointRegistration>();
    }
}