using Expenso.Shared.Tests.UnitTests.ModuleDefinition.Extensions.EndpointRegistrationExtensions;
using Expenso.Shared.Tests.Utils.UnitTests;
using Expenso.Shared.Types.Exceptions;
using Expenso.Shared.UserContext;

namespace Expenso.Shared.Tests.ArchTests;

internal static class Assemblies
{
    private static readonly Assembly ModuleDefinition = typeof(ModuleDefinition.ModuleDefinition).Assembly;
    private static readonly Assembly Types = typeof(NotFoundException).Assembly;
    private static readonly Assembly UserContext = typeof(IUserContext).Assembly;
    private static readonly Assembly UnitTests = typeof(EndpointRegistrationExtensionsTestBase).Assembly;
    private static readonly Assembly UnitTestsUtils = typeof(TestBase<>).Assembly;
    private static readonly Assembly ArchTests = typeof(Assemblies).Assembly;
    private static readonly Assembly ArchTestsUtils = typeof(ArchTestTestBase).Assembly;

    public static IReadOnlyCollection<Assembly> ToArray()
    {
        List<Assembly> assemblies =
        [
            ModuleDefinition,
            Types,
            UserContext,
            UnitTests,
            UnitTestsUtils,
            ArchTests,
            ArchTestsUtils
        ];

        return assemblies;
    }
}