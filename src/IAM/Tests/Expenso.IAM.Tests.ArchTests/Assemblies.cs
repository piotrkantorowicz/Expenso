using Expenso.IAM.Api;
using Expenso.IAM.Core.Services;
using Expenso.IAM.Proxy.DTO;
using Expenso.IAM.Tests.ArchTests.AccessModifiers;
using Expenso.IAM.Tests.UnitTests.Services;

namespace Expenso.IAM.Tests.ArchTests;

internal static class Assemblies
{
    public static readonly Assembly Api = typeof(IamModule).Assembly;
    public static readonly Assembly Core = typeof(UserService).Assembly;
    public static readonly Assembly Proxy = typeof(UserDto).Assembly;
    public static readonly Assembly UnitTests = typeof(UserServiceTestBase).Assembly;
    public static readonly Assembly ArchTests = typeof(AccessModifierTests).Assembly;

    public static Assembly[] ToArray()
    {
        Assembly[] assemblies = new List<Assembly>
        {
            Api,
            Core,
            Proxy,
            UnitTests,
            ArchTests
        }.ToArray();

        return assemblies;
    }
}