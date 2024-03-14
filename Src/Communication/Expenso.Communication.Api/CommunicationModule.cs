using System.Reflection;

using Expenso.Communication.Core;
using Expenso.Communication.Proxy;
using Expenso.Shared.System.Modules;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Expenso.Communication.Api;

public sealed class CommunicationModule : ModuleDefinition
{
    public override string ModulePrefix => "/communication";

    public override IReadOnlyCollection<Assembly> GetModuleAssemblies()
    {
        return new List<Assembly>
        {
            typeof(CommunicationModule).Assembly,
            typeof(Extensions).Assembly,
            typeof(ICommunicationProxy).Assembly
        };
    }

    public override void AddDependencies(IServiceCollection services, IConfiguration configuration)
    {
        services.AddCommunicationCore(GetModuleAssemblies());
    }

    public override IReadOnlyCollection<EndpointRegistration> CreateEndpoints()
    {
        return Array.Empty<EndpointRegistration>();
    }
}