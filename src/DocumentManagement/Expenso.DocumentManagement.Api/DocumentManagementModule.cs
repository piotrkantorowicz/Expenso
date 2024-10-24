using System.Reflection;

using Expenso.DocumentManagement.Core;
using Expenso.DocumentManagement.Shared;
using Expenso.Shared.System.Modules;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Extensions = Expenso.DocumentManagement.Core.Extensions;

namespace Expenso.DocumentManagement.Api;

public sealed class DocumentManagementModule : IModuleDefinition
{
    public string ModulePrefix => "/document-management";

    public IReadOnlyCollection<Assembly> GetModuleAssemblies()
    {
        return
        [
            typeof(DocumentManagementModule).Assembly,
            typeof(Extensions).Assembly,
            typeof(IDocumentManagementProxy).Assembly
        ];
    }

    public void AddDependencies(IServiceCollection services, IConfiguration configuration)
    {
        services.AddDocumentManagementCore(configuration: configuration);
        services.AddDocumentManagementProxy(assemblies: GetModuleAssemblies());
    }

    public IReadOnlyCollection<EndpointRegistration> CreateEndpoints()
    {
        return Array.Empty<EndpointRegistration>();
    }
}