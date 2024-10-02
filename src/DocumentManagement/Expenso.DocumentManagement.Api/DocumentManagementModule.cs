using System.Reflection;

using Expenso.DocumentManagement.Core;
using Expenso.DocumentManagement.Proxy;
using Expenso.Shared.System.Modules;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Swashbuckle.AspNetCore.SwaggerGen;

using CoreExtensions = Expenso.DocumentManagement.Core.Extensions;

namespace Expenso.DocumentManagement.Api;

public sealed class DocumentManagementModule : IModuleDefinition
{
    public string ModulePrefix => "/document-management";

    public IReadOnlyCollection<Assembly> GetModuleAssemblies()
    {
        return
        [
            typeof(DocumentManagementModule).Assembly,
            typeof(CoreExtensions).Assembly,
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

    public void ConfigureSwaggerOptions(SwaggerGenOptions options)
    {
    }
}