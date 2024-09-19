using Expenso.Shared.Tests.UnitTests.System.Modules.Extensions.EndpointRegistrationExtensions;
using Expenso.Shared.Tests.Utils.UnitTests;

namespace Expenso.Shared.Tests.ArchTests.Assemblies;

internal static class TestsAssemblies
{
    private static readonly Assembly UnitTests = typeof(EndpointRegistrationExtensionsTestBase).Assembly;
    private static readonly Assembly UnitTestsUtils = typeof(TestBase<>).Assembly;
    private static readonly Assembly ArchTests = typeof(TestsAssemblies).Assembly;
    private static readonly Assembly ArchTestsUtils = typeof(ArchTestTestBase).Assembly;

    public static IReadOnlyCollection<Assembly> ToArray()
    {
        List<Assembly> assemblies =
        [
            UnitTests,
            UnitTestsUtils,
            ArchTests,
            ArchTestsUtils
        ];

        return assemblies;
    }
}