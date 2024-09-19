using System.Reflection;

using Expenso.BudgetSharing.Api;
using Expenso.BudgetSharing.Domain.BudgetPermissions;
using Expenso.BudgetSharing.Proxy;
using Expenso.BudgetSharing.Tests.UnitTests.Domain;

namespace Expenso.BudgetSharing.Tests.ArchTests;

internal static class Assemblies
{
    private static readonly Assembly Api = typeof(BudgetSharingModule).Assembly;
    private static readonly Assembly Core = typeof(BudgetPermission).Assembly;
    private static readonly Assembly Proxy = typeof(IBudgetSharingProxy).Assembly;
    private static readonly Assembly UnitTests = typeof(DomainTestBase<>).Assembly;
    private static readonly Assembly ArchTests = typeof(Assemblies).Assembly;

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