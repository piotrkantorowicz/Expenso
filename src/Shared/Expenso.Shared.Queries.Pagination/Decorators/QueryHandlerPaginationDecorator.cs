using Expenso.Shared.System.Types.Pagination.Constants;

namespace Expenso.Shared.Queries.Pagination.Decorators;

internal sealed class QueryHandlerPaginationDecorator<TQuery, TResult> : IQueryHandler<TQuery, TResult>
    where TQuery : class, IQuery<TResult> where TResult : class
{
    private readonly IQueryHandler<TQuery, TResult> _decorated;

    public QueryHandlerPaginationDecorator(IQueryHandler<TQuery, TResult> decorated)
    {
        _decorated = decorated ?? throw new ArgumentNullException(paramName: nameof(decorated));
    }

    public async Task<TResult?> HandleAsync(TQuery query, CancellationToken cancellationToken = default)
    {
        if (query is not IPagedQuery pagedQuery)
        {
            return await _decorated.HandleAsync(query: query, cancellationToken: cancellationToken);
        }

        if (pagedQuery.Pagination?.Page <= 0)
        {
            pagedQuery.Pagination.Page = 1;
        }

        if (pagedQuery.Pagination?.Limit <= 0)
        {
            pagedQuery.Pagination.Limit = PaginationDefaults.Limit;
        }

        if (pagedQuery.Pagination?.Limit > PaginationDefaults.MaxLimit)
        {
            pagedQuery.Pagination.Limit = PaginationDefaults.MaxLimit;
        }

        return await _decorated.HandleAsync(query: query, cancellationToken: cancellationToken);
    }
}