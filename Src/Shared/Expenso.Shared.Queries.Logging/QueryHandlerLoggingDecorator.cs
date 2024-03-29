using System.Diagnostics;

using Expenso.Shared.System.Logging;
using Expenso.Shared.System.Serialization;

using Microsoft.Extensions.Logging;

namespace Expenso.Shared.Queries.Logging;

internal sealed class QueryHandlerLoggingDecorator<TQuery, TResult>(
    ILogger<QueryHandlerLoggingDecorator<TQuery, TResult>> logger,
    IQueryHandler<TQuery, TResult> decorated,
    ISerializer serializer) : IQueryHandler<TQuery, TResult> where TQuery : class, IQuery<TResult> where TResult : class
{
    private readonly IQueryHandler<TQuery, TResult> _decorated =
        decorated ?? throw new ArgumentNullException(nameof(decorated));

    private readonly ILogger<QueryHandlerLoggingDecorator<TQuery, TResult>> _logger =
        logger ?? throw new ArgumentNullException(nameof(logger));

    private readonly ISerializer _serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));

    public async Task<TResult?> HandleAsync(TQuery query, CancellationToken cancellationToken)
    {
        EventId executing = LoggingUtils.QueryExecuting;
        EventId executed = LoggingUtils.QueryExecuted;
        string? queryName = query.GetType().FullName;
        string serializedQuery = _serializer.Serialize(query);
        _logger.LogInformation(executing, "[START] {QueryName}. Params: {SerializedQuery}", queryName, serializedQuery);
        Stopwatch stopwatch = new();

        try
        {
            stopwatch.Start();
            TResult? result = await _decorated.HandleAsync(query, cancellationToken);
            stopwatch.Stop();

            _logger.LogInformation(executed, "[END] {QueryName}: {ExecutionTime}[ms]", queryName,
                stopwatch.ElapsedMilliseconds);

            return result;
        }
        catch (Exception ex)
        {
            stopwatch.Stop();

            _logger.LogError(LoggingUtils.UnexpectedException, ex, "[END] {QueryName}: {ExecutionTime}[ms]", queryName,
                stopwatch.ElapsedMilliseconds);

            throw;
        }
    }
}