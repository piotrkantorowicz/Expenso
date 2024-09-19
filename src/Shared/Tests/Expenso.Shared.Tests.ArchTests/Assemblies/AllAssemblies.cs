namespace Expenso.Shared.Tests.ArchTests.Assemblies;

internal static class AllAssemblies
{
    public static Assembly[] ToArray()
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
            .ToArray();
    }
}