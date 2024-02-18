using Expenso.Api.Tests.ArchTests.AccessModifiers;
using Expenso.Api.Tests.E2E.Configuration;
using Expenso.Api.Tests.UnitTests.Configuration.Execution.ExecutionContextAccessor;

namespace Expenso.Api.Tests.ArchTests;

internal static class Assemblies
{
    private static readonly Assembly Api = typeof(Program).Assembly;
    private static readonly Assembly UnitTests = typeof(ExecutionContextAccessorTestBase).Assembly;
    private static readonly Assembly E2E = typeof(WebApp).Assembly;
    private static readonly Assembly ArchTests = typeof(AccessModifierTests).Assembly;

    public static IReadOnlyCollection<Assembly> ToArray()
    {
        Assembly[] assemblies = new List<Assembly>
        {
            Api,
            UnitTests,
            E2E,
            ArchTests
        }.ToArray();

        return assemblies;
    }
}