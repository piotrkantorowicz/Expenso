using System.Reflection;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Expenso.Shared.System.Modules;

public interface IModuleDefinition
{
    string ModuleName => GetType().Name;

    string ModulePrefix { get; }

    IReadOnlyCollection<Assembly> GetModuleAssemblies();

    void AddDependencies(IServiceCollection services, IConfiguration configuration);

    IReadOnlyCollection<EndpointRegistration> CreateEndpoints();
}