using Expenso.Shared.Queries;
using Expenso.Shared.Queries.Logging;

namespace Expenso.Shared.Tests.ArchTests.AccessModifiers.Assemblies;

internal static class QueriesAssemblies
{
    private static readonly Assembly Queries = typeof(IQuery).Assembly;
    private static readonly Assembly QueriesLogging = typeof(QueryHandlerLoggingDecorator<,>).Assembly;

    private static readonly Dictionary<string, Assembly> Assemblies = new()
    {
        [key: nameof(Queries)] = Queries,
        [key: nameof(QueriesLogging)] = QueriesLogging
    };

    public static IReadOnlyDictionary<string, Assembly> GetAssemblies()
    {
        return Assemblies;
    }
}