using System.Reflection;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Expenso.Shared.ModuleDefinition;

public abstract class ModuleDefinition
{
    public string ModuleName => GetType().Name;

    public abstract string ModulePrefix { get; }

    public abstract IReadOnlyCollection<Assembly> GetModuleAssemblies();

    public abstract void AddDependencies(IServiceCollection services, IConfiguration configuration);

    public abstract IReadOnlyCollection<EndpointRegistration> CreateEndpoints();
}