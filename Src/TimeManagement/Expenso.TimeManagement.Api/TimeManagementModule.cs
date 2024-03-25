using System.Reflection;

using Expenso.Shared.System.Modules;
using Expenso.TimeManagement.Core;
using Expenso.TimeManagement.Proxy;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using CoreExtensions = Expenso.TimeManagement.Core.Extensions;

namespace Expenso.TimeManagement.Api;

public class TimeManagementModule : ModuleDefinition
{
    public override string ModulePrefix => "/document-management";

    public override IReadOnlyCollection<Assembly> GetModuleAssemblies()
    {
        return
        [
            typeof(TimeManagementModule).Assembly,
            typeof(CoreExtensions).Assembly,
            typeof(ITimeManagementProxy).Assembly
        ];
    }

    public override void AddDependencies(IServiceCollection services, IConfiguration configuration)
    {
        services.AddTimeManagementCore(configuration, ModuleName);
        services.AddTimeManagementProxy(GetModuleAssemblies());
    }

    public override IReadOnlyCollection<EndpointRegistration> CreateEndpoints()
    {
        return Array.Empty<EndpointRegistration>();
    }
}