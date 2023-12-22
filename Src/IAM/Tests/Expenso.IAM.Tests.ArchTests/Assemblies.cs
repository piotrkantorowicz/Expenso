using Expenso.IAM.Api;
using Expenso.IAM.Core.Services;
using Expenso.IAM.Proxy.Contracts;
using Expenso.IAM.Tests.ArchTests.AccessModifiers;
using Expenso.IAM.Tests.UnitTests.Services;

namespace Expenso.IAM.Tests.ArchTests;

internal static class Assemblies
{
    private static readonly Assembly Api = typeof(IamModule).Assembly;
    private static readonly Assembly Core = typeof(UserService).Assembly;
    private static readonly Assembly Proxy = typeof(UserContract).Assembly;
    private static readonly Assembly UnitTests = typeof(UserServiceTestBase).Assembly;
    private static readonly Assembly ArchTests = typeof(AccessModifierTests).Assembly;

    public static IReadOnlyCollection<Assembly> ToArray()
    {
        List<Assembly> assemblies = new()
        {
            Api,
            Core,
            Proxy,
            UnitTests,
            ArchTests
        };

        return assemblies;
    }
}