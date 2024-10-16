﻿namespace Expenso.Shared.Tests.ArchTests.AccessModifiers.Assemblies;

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
}