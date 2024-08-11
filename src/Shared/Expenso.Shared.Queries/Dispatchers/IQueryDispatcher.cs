namespace Expenso.Shared.Queries.Dispatchers;

public interface IQueryDispatcher
{
    Task<TResult?> QueryAsync<TResult>(IQuery<TResult> query, CancellationToken cancellationToken)
        where TResult : class;
}