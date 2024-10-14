using Expenso.UserPreferences.Api;
using Expenso.UserPreferences.Core.Domain.Preferences.Repositories;
using Expenso.UserPreferences.Shared;
using Expenso.UserPreferences.Tests.ArchTests.AccessModifiers;
using Expenso.UserPreferences.Tests.UnitTests.Persistence.EfCore.Repositories.PreferencesRepository;

namespace Expenso.UserPreferences.Tests.ArchTests;

internal static class Assemblies
{
    private static readonly Assembly Api = typeof(UserPreferencesModule).Assembly;
    private static readonly Assembly Core = typeof(IPreferencesRepository).Assembly;
    private static readonly Assembly Proxy = typeof(IUserPreferencesProxy).Assembly;
    private static readonly Assembly UnitTests = typeof(PreferenceRepositoryTestBase).Assembly;
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