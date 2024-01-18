using Expenso.UserPreferences.Api;
using Expenso.UserPreferences.Core.Application.Services;
using Expenso.UserPreferences.Proxy;
using Expenso.UserPreferences.Tests.ArchTests.AccessModifiers;
using Expenso.UserPreferences.Tests.UnitTests.Services;

namespace Expenso.UserPreferences.Tests.ArchTests;

internal static class Assemblies
{
    private static readonly Assembly Api = typeof(UserPreferencesModule).Assembly;
    private static readonly Assembly Core = typeof(IPreferencesService).Assembly;
    private static readonly Assembly Proxy = typeof(IUserPreferencesProxy).Assembly;
    private static readonly Assembly UnitTests = typeof(PreferenceServiceTestBase).Assembly;
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