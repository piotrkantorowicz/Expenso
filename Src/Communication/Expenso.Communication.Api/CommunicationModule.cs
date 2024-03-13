using System.Reflection;

using Expenso.Communication.Core;
using Expenso.Communication.Proxy;
using Expenso.Shared.System.Modules;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using CoreServiceCollectionExtensions = Expenso.Communication.Core.ServiceCollectionExtensions;

namespace Expenso.Communication.Api;

public sealed class CommunicationModule : ModuleDefinition
{
    public override string ModulePrefix => "/communication";

    public override IReadOnlyCollection<Assembly> GetModuleAssemblies()
    {
        return new List<Assembly>
        {
            typeof(CommunicationModule).Assembly,
            typeof(CoreServiceCollectionExtensions).Assembly,
            typeof(ICommunicationProxy).Assembly
        };
    }

    public override void AddDependencies(IServiceCollection services, IConfiguration configuration)
    {
        services.AddCommunicationCore();
    }

    public override IReadOnlyCollection<EndpointRegistration> CreateEndpoints()
    {
        return Array.Empty<EndpointRegistration>();
    }
}