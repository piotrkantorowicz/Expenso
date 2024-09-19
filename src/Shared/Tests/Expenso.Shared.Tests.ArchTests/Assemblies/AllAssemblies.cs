namespace Expenso.Shared.Tests.ArchTests.Assemblies;

internal static class AllAssemblies
{
    public static IReadOnlyCollection<Assembly> GetAssembliesCollection()
    {
        return new[]
            {
                CommandsAssemblies.GetAssemblies(),
                DatabaseAssemblies.GetAssemblies(),
                DomainAssemblies.GetAssemblies(),
                IntegrationAssemblies.GetAssemblies(),
                QueriesAssemblies.GetAssemblies(),
                SystemAssemblies.GetAssemblies(),
                TestsAssemblies.GetAssemblies()
            }
            .SelectMany(selector: assembly => assembly.Values)
            .ToList()
            .AsReadOnly();
    }

    public static IReadOnlyDictionary<string, Assembly> GetAssembliesDictionary()
    {
        return new[]
            {
                CommandsAssemblies.GetAssemblies(),
                DatabaseAssemblies.GetAssemblies(),
                DomainAssemblies.GetAssemblies(),
                IntegrationAssemblies.GetAssemblies(),
                QueriesAssemblies.GetAssemblies(),
                SystemAssemblies.GetAssemblies(),
                TestsAssemblies.GetAssemblies()
            }
            .SelectMany(selector: assembly => assembly)
            .ToDictionary(keySelector: x => x.Key, elementSelector: y => y.Value);
    }
}