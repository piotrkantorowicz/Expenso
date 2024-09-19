using Expenso.Shared.System.Configuration;
using Expenso.Shared.System.Expressions;
using Expenso.Shared.System.Logging;
using Expenso.Shared.System.Metrics;
using Expenso.Shared.System.Modules;
using Expenso.Shared.System.Serialization;
using Expenso.Shared.System.Types.Clock;

using SerilogExtensions = Expenso.Shared.System.Logging.Serilog.Extensions;
using TaskExtensions = Expenso.Shared.System.Tasks.TaskExtensions;

namespace Expenso.Shared.Tests.ArchTests.Assemblies;

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

    private static readonly Dictionary<string, Assembly> Assemblies = new()
    {
        [key: nameof(Configuration)] = Configuration,
        [key: nameof(Expressions)] = Expressions,
        [key: nameof(Logging)] = Logging,
        [key: nameof(LoggingSerilog)] = LoggingSerilog,
        [key: nameof(Metrics)] = Metrics,
        [key: nameof(Modules)] = Modules,
        [key: nameof(Serialization)] = Serialization,
        [key: nameof(Tasks)] = Tasks,
        [key: nameof(Types)] = Types
    };

    public static IReadOnlyDictionary<string, Assembly> GetAssemblies()
    {
        return Assemblies;
    }
}