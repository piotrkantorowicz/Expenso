namespace Expenso.Shared.Queries;

public interface IQueryDispatcher
{
    Task<TResult?> QueryAsync<TResult>(IQuery<TResult> query, CancellationToken cancellationToken = default)
        where TResult : class;
}