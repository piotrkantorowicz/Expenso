using System.Reflection;

using Expenso.TimeManagement.Api;
using Expenso.TimeManagement.Core.Application.Jobs.Shared.BackgroundJobs;
using Expenso.TimeManagement.Proxy;
using Expenso.TimeManagement.Tests.ArchTests.AccessModifiers;
using Expenso.TimeManagement.Tests.UnitTests;

namespace Expenso.TimeManagement.Tests.ArchTests;

internal static class Assemblies
{
    private static readonly Assembly Api = typeof(TimeManagementModule).Assembly;
    private static readonly Assembly Core = typeof(BudgetSharingRequestsExpirationJob).Assembly;
    private static readonly Assembly Proxy = typeof(ITimeManagementProxy).Assembly;
    private static readonly Assembly UnitTests = typeof(BudgetSharingRequestsExpirationJobTestBase).Assembly;
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