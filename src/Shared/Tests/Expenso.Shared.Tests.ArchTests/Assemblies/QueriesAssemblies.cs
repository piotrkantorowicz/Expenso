using Expenso.Shared.Queries;
using Expenso.Shared.Queries.Logging;

namespace Expenso.Shared.Tests.ArchTests.Assemblies;

internal static class QueriesAssemblies
{
    private static readonly Assembly Queries = typeof(IQuery).Assembly;
    private static readonly Assembly QueriesLogging = typeof(QueryHandlerLoggingDecorator<,>).Assembly;

    public static IReadOnlyCollection<Assembly> GetAssemblies()
    {
        List<Assembly> assemblies =
        [
            Queries,
            QueriesLogging
        ];

        return assemblies;
    }
}