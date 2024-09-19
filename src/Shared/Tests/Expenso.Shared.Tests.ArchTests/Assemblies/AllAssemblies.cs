namespace Expenso.Shared.Tests.ArchTests.Assemblies;

internal static class AllAssemblies
{
    public static Assembly[] ToArray()
    {
        return CommandsAssemblies
            .ToArray()
            .Concat(second: DatabaseAssemblies.ToArray())
            .Concat(second: DomainAssemblies.ToArray())
            .Concat(second: IntegrationAssemblies.ToArray())
            .Concat(second: QueriesAssemblies.ToArray())
            .Concat(second: SystemAssemblies.ToArray())
            .Concat(second: TestsAssemblies.ToArray())
            .ToArray();
    }
}