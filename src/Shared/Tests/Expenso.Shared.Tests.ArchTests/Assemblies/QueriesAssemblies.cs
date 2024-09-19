using Expenso.Shared.Queries;
using Expenso.Shared.Queries.Logging;

namespace Expenso.Shared.Tests.ArchTests.Assemblies;

internal static class QueriesAssemblies
{
    private static readonly Assembly Queries = typeof(IQuery).Assembly;
    private static readonly Assembly QueriesLogging = typeof(QueryHandlerLoggingDecorator<,>).Assembly;

    public static IReadOnlyCollection<Assembly> ToArray()
    {
        List<Assembly> assemblies =
        [
            Queries,
            QueriesLogging
        ];

        return assemblies;
    }
}