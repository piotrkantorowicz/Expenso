using Expenso.Shared.Tests.UnitTests.ModuleDefinition.Extensions.EndpointRegistrationExtensions;
using Expenso.Shared.Types.Exceptions;
using Expenso.Shared.UserContext;

namespace Expenso.Shared.Tests.ArchTests;

internal static class Assemblies
{
    public static readonly Assembly ModuleDefinition = typeof(ModuleDefinition.ModuleDefinition).Assembly;
    public static readonly Assembly Types = typeof(NotFoundException).Assembly;
    public static readonly Assembly UserContext = typeof(IUserContext).Assembly;
    public static readonly Assembly UnitTests = typeof(EndpointRegistrationExtensionsTestBase).Assembly;
    public static readonly Assembly UnitTestsUtils = typeof(UnitTestTestBase).Assembly;
    public static readonly Assembly ArchTests = typeof(Assemblies).Assembly;
    public static readonly Assembly ArchTestsUtils = typeof(ArchTestTestBase).Assembly;

    public static Assembly[] ToArray()
    {
        Assembly[] assemblies = new List<Assembly>
        {
            ModuleDefinition,
            Types,
            UserContext,
            UnitTests,
            UnitTestsUtils,
            ArchTests,
            ArchTestsUtils
        }.ToArray();

        return assemblies;
    }
}