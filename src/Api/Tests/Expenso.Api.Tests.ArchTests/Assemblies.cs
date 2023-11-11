using Expenso.Api.Tests.ArchTests.AccessModifiers;
using Expenso.Api.Tests.E2E.Configuration;
using Expenso.Api.Tests.UnitTests.Configuration.Auth.Users.UserContext.UserContextAccessor;

namespace Expenso.Api.Tests.ArchTests;

internal static class Assemblies
{
    public static readonly Assembly Api = typeof(Program).Assembly;
    public static readonly Assembly UnitTests = typeof(UserContextAccessorTestBase).Assembly;
    public static readonly Assembly E2E = typeof(WebApp).Assembly;
    public static readonly Assembly ArchTests = typeof(AccessModifierTests).Assembly;

    public static Assembly[] ToArray()
    {
        Assembly[] assemblies = new List<Assembly> { Api, UnitTests, E2E, ArchTests }.ToArray();

        return assemblies;
    }
}