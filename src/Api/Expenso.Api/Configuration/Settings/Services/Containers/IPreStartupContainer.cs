using System.Reflection;

namespace Expenso.Api.Configuration.Settings.Services.Containers;

internal interface IPreStartupContainer
{
    void Build(IConfiguration configuration, Assembly[] assemblies);

    T? Resolve<T>();

    IReadOnlyCollection<T> ResolveMany<T>();
}