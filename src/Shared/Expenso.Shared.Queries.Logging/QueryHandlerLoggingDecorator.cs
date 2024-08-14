using System.Diagnostics;

using Expenso.Shared.System.Logging;
using Expenso.Shared.System.Serialization;

using Microsoft.Extensions.Logging;

namespace Expenso.Shared.Queries.Logging;

internal sealed class QueryHandlerLoggingDecorator<TQuery, TResult> : IQueryHandler<TQuery, TResult>
    where TQuery : class, IQuery<TResult> where TResult : class
{
    private readonly IQueryHandler<TQuery, TResult> _decorated;
    private readonly ILogger<QueryHandlerLoggingDecorator<TQuery, TResult>> _logger;
    private readonly ISerializer _serializer;

    public QueryHandlerLoggingDecorator(ILogger<QueryHandlerLoggingDecorator<TQuery, TResult>> logger,
        IQueryHandler<TQuery, TResult> decorated, ISerializer serializer)
    {
        _decorated = decorated ?? throw new ArgumentNullException(paramName: nameof(decorated));
        _logger = logger ?? throw new ArgumentNullException(paramName: nameof(logger));
        _serializer = serializer ?? throw new ArgumentNullException(paramName: nameof(serializer));
    }

    public async Task<TResult?> HandleAsync(TQuery query, CancellationToken cancellationToken)
    {
        EventId executing = LoggingUtils.QueryExecuting;
        EventId executed = LoggingUtils.QueryExecuted;
        string? queryName = query.GetType().FullName;
        string serializedQuery = _serializer.Serialize(value: query);

        _logger.LogInformation(eventId: executing, message: "[START] {QueryName}. Params: {SerializedQuery}", queryName,
            serializedQuery);

        Stopwatch stopwatch = new();

        try
        {
            stopwatch.Start();
            TResult? result = await _decorated.HandleAsync(query: query, cancellationToken: cancellationToken);
            stopwatch.Stop();

            _logger.LogInformation(eventId: executed, message: "[END] {QueryName}: {ExecutionTime}[ms]", queryName,
                stopwatch.ElapsedMilliseconds);

            return result;
        }
        catch (Exception ex)
        {
            stopwatch.Stop();

            _logger.LogError(eventId: LoggingUtils.UnexpectedException, exception: ex,
                message: "[END] {QueryName}: {ExecutionTime}[ms]", queryName, stopwatch.ElapsedMilliseconds);

            throw;
        }
    }
}