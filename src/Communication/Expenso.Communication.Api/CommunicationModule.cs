using System.Reflection;

using Expenso.Communication.Core;
using Expenso.Communication.Proxy;
using Expenso.Shared.System.Modules;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using CoreExtensions = Expenso.Communication.Core.Extensions;

namespace Expenso.Communication.Api;

public sealed class CommunicationModule : IModuleDefinition
{
    public string ModulePrefix => "/communication";

    public IReadOnlyCollection<Assembly> GetModuleAssemblies()
    {
        return new List<Assembly>
        {
            typeof(CommunicationModule).Assembly,
            typeof(CoreExtensions).Assembly,
            typeof(ICommunicationProxy).Assembly
        };
    }

    public void AddDependencies(IServiceCollection services, IConfiguration configuration)
    {
        services.AddCommunicationCore(assemblies: GetModuleAssemblies());
        services.AddCommunicationProxy(assemblies: GetModuleAssemblies());
    }

    public IReadOnlyCollection<EndpointRegistration> CreateEndpoints()
    {
        return Array.Empty<EndpointRegistration>();
    }
}