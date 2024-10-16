using Expenso.IAM.Api;
using Expenso.IAM.Core.Application.Users.Read.Services.Acl.Keycloak;
using Expenso.IAM.Shared;
using Expenso.IAM.Tests.ArchTests.AccessModifiers;
using Expenso.IAM.Tests.UnitTests.Users.Services.Acl.Keycloak;

namespace Expenso.IAM.Tests.ArchTests;

internal static class Assemblies
{
    private static readonly Assembly Api = typeof(IamModule).Assembly;
    private static readonly Assembly Core = typeof(UserService).Assembly;
    private static readonly Assembly Proxy = typeof(IIamProxy).Assembly;
    private static readonly Assembly UnitTests = typeof(UserServiceTestBase).Assembly;
    private static readonly Assembly ArchTests = typeof(AccessModifierTests).Assembly;

    public static IReadOnlyCollection<Assembly> GetAssemblies()
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