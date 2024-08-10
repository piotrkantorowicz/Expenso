using System.Reflection;

using Expenso.TimeManagement.Api;
using Expenso.TimeManagement.Core.Application.Jobs.Shared.BackgroundJobs;
using Expenso.TimeManagement.Proxy;
using Expenso.TimeManagement.Tests.ArchTests.AccessModifiers;
using Expenso.TimeManagement.Tests.UnitTests;
using Expenso.TimeManagement.Tests.UnitTests.Application.Jobs.Shared.BackgroundJobs.JobExecutions;

namespace Expenso.TimeManagement.Tests.ArchTests;

internal static class Assemblies
{
    private static readonly Assembly Api = typeof(TimeManagementModule).Assembly;
    private static readonly Assembly Core = typeof(BackgroundJob).Assembly;
    private static readonly Assembly Proxy = typeof(ITimeManagementProxy).Assembly;
    private static readonly Assembly UnitTests = typeof(JobExecutionTestBase).Assembly;
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