using Expenso.Shared.System.Configuration;
using Expenso.Shared.System.Expressions;
using Expenso.Shared.System.Logging;
using Expenso.Shared.System.Metrics;
using Expenso.Shared.System.Modules;
using Expenso.Shared.System.Serialization;
using Expenso.Shared.System.Types.Clock;

using Extensions = Expenso.Shared.System.Logging.Serilog.Extensions;

namespace Expenso.Shared.Tests.ArchTests.Assemblies;

using SerilogExtensions = Extensions;

internal static class SystemAssemblies
{
    private static readonly Assembly Configuration = typeof(OptionsExtensions).Assembly;
    private static readonly Assembly Expressions = typeof(ReplaceExpressionVisitor).Assembly;
    private static readonly Assembly Logging = typeof(ILoggerService<>).Assembly;
    private static readonly Assembly LoggingSerilog = typeof(SerilogExtensions).Assembly;
    private static readonly Assembly Metrics = typeof(OtlpSettings).Assembly;
    private static readonly Assembly Modules = typeof(IModuleDefinition).Assembly;
    private static readonly Assembly Serialization = typeof(ISerializer).Assembly;
    private static readonly Assembly Tasks = typeof(TaskExtensions).Assembly;
    private static readonly Assembly Types = typeof(IClock).Assembly;

    public static IReadOnlyCollection<Assembly> ToArray()
    {
        List<Assembly> assemblies =
        [
            Configuration,
            Expressions,
            Logging,
            LoggingSerilog,
            Metrics,
            Modules,
            Serialization,
            Tasks
        ];

        return assemblies;
    }
}