using System.Reflection;

using Expenso.Shared.System.Configuration;
using Expenso.Shared.System.Expressions;
using Expenso.Shared.System.Logging;
using Expenso.Shared.System.Metrics.Settings;
using Expenso.Shared.System.Modules;
using Expenso.Shared.System.Modules.Constants;
using Expenso.Shared.System.Serialization;
using Expenso.Shared.System.Time;

using Extensions = Expenso.Shared.System.Logging.Serilog.Extensions;
using IClock = Expenso.Shared.System.Types.Clock.IClock;
using TaskExtensions = Expenso.Shared.System.Tasks.TaskExtensions;

namespace Expenso.Shared.Tests.ArchTests.AccessModifiers.Assemblies;

internal static class SystemAssemblies
{
    private static readonly Assembly Configuration = typeof(OptionsExtensions).Assembly;
    private static readonly Assembly Expressions = typeof(ReplaceExpressionVisitor).Assembly;
    private static readonly Assembly Logging = typeof(ILoggerService<>).Assembly;
    private static readonly Assembly LoggingSerilog = typeof(Extensions).Assembly;
    private static readonly Assembly Metrics = typeof(OtlpSettings).Assembly;
    private static readonly Assembly Modules = typeof(IModuleDefinition).Assembly;
    private static readonly Assembly ModulesNames = typeof(ModuleNames).Assembly;
    private static readonly Assembly Serialization = typeof(ISerializer).Assembly;
    private static readonly Assembly Tasks = typeof(TaskExtensions).Assembly;
    private static readonly Assembly Time = typeof(ITimeZoneClock).Assembly;
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
        [key: nameof(Time)] = Time,
        [key: nameof(Types)] = Types,
        [key: nameof(ModulesNames)] = ModulesNames
    };

    public static IReadOnlyDictionary<string, Assembly> GetAssemblies()
    {
        return Assemblies;
    }
}