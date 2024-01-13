using Expenso.IAM.Api;
using Expenso.IAM.Core.Services.KeycloakAcl;
using Expenso.IAM.Proxy.Contracts;
using Expenso.IAM.Tests.ArchTests.AccessModifiers;
using Expenso.IAM.Tests.UnitTests.Services;

namespace Expenso.IAM.Tests.ArchTests;

internal static class Assemblies
{
    private static readonly Assembly Api = typeof(IamModule).Assembly;
    private static readonly Assembly Core = typeof(KeycloakAclUserService).Assembly;
    private static readonly Assembly Proxy = typeof(UserContract).Assembly;
    private static readonly Assembly UnitTests = typeof(KeycloakAclUserServiceTestBase).Assembly;
    private static readonly Assembly ArchTests = typeof(AccessModifierTests).Assembly;

    public static IReadOnlyCollection<Assembly> ToArray()
    {
        List<Assembly> assemblies =
        [
            Api,
            Core,
            Proxy,
            UnitTests,
            ArchTests
        ];

        return assemblies;
    }
}