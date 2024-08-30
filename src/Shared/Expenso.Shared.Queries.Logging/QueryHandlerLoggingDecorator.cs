using System.Diagnostics;

using Expenso.Shared.System.Logging;
using Expenso.Shared.System.Serialization;

namespace Expenso.Shared.Queries.Logging;

internal sealed class QueryHandlerLoggingDecorator<TQuery, TResult> : IQueryHandler<TQuery, TResult>
    where TQuery : class, IQuery<TResult> where TResult : class
{
    private readonly IQueryHandler<TQuery, TResult> _decorated;
    private readonly ILoggerService<QueryHandlerLoggingDecorator<TQuery, TResult>> _logger;
    private readonly ISerializer _serializer;

    public QueryHandlerLoggingDecorator(ILoggerService<QueryHandlerLoggingDecorator<TQuery, TResult>> logger,
        IQueryHandler<TQuery, TResult> decorated, ISerializer serializer)
    {
        _decorated = decorated ?? throw new ArgumentNullException(paramName: nameof(decorated));
        _logger = logger ?? throw new ArgumentNullException(paramName: nameof(logger));
        _serializer = serializer ?? throw new ArgumentNullException(paramName: nameof(serializer));
    }

    public async Task<TResult?> HandleAsync(TQuery query, CancellationToken cancellationToken)
    {
        string? queryName = query.GetType().FullName;
        string serializedQuery = _serializer.Serialize(value: query);

        _logger.LogInfo(eventId: LoggingUtils.QueryExecuting, message: "[START] {QueryName}. Params: {SerializedQuery}",
            messageContext: query.MessageContext, queryName, serializedQuery);

        Stopwatch stopwatch = new();

        try
        {
            stopwatch.Start();
            TResult? result = await _decorated.HandleAsync(query: query, cancellationToken: cancellationToken);
            stopwatch.Stop();

            _logger.LogInfo(eventId: LoggingUtils.QueryExecuted, message: "[END] {QueryName}: {ExecutionTime}[ms]",
                messageContext: query.MessageContext, queryName, stopwatch.ElapsedMilliseconds);

            return result;
        }
        catch (Exception ex)
        {
            stopwatch.Stop();

            _logger.LogError(eventId: LoggingUtils.UnexpectedError, message: "[END] {QueryName}: {ExecutionTime}[ms]",
                exception: ex, messageContext: query.MessageContext, queryName, stopwatch.ElapsedMilliseconds);

            throw;
        }
    }
}