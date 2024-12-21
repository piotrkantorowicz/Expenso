using System.Linq.Expressions;

using Expenso.Shared.System.Types.Pagination;
using Expenso.Shared.System.Types.Pagination.Constants;

using Microsoft.EntityFrameworkCore;

namespace Expenso.Shared.Database.EfCore.Queryable;

public static class QueryableExtensions
{
    public static IQueryable<T> Tracking<T>(this IQueryable<T> queryable, bool? useTracking) where T : class
    {
        return useTracking is true ? queryable : queryable.AsNoTracking();
    }

    public static async Task<IPagedList<T>> PaginationAsync<T>(this IQueryable<T> queryable,
        Expression<Func<T, bool>> filter, Paging? pagination, CancellationToken cancellationToken) where T : class
    {
        int page = Math.Max(val1: 1, val2: pagination?.Page ?? PaginationDefaults.Page);
        int limit = Math.Max(val1: 1, val2: pagination?.Limit ?? PaginationDefaults.Limit);
        int allRecordsCount = await queryable.Where(predicate: filter).CountAsync(cancellationToken: cancellationToken);
        int maxPage = (int)Math.Ceiling(a: allRecordsCount / (double)limit);
        page = Math.Min(val1: page, val2: maxPage);

        IReadOnlyCollection<T> budgetPermissionRequests = await queryable
            .Where(predicate: filter)
            .Skip(count: limit * (page - 1))
            .Take(count: limit)
            .ToListAsync(cancellationToken: cancellationToken);

        int totalPages = (int)Math.Ceiling(a: allRecordsCount / (double)limit);
        int currentPage = Math.Min(val1: totalPages, val2: page);

        return PagedList<T>.Create(items: budgetPermissionRequests, currentPage: currentPage, resultsPerPage: limit,
            totalPages: Math.Max(val1: 1, val2: totalPages), totalResults: allRecordsCount);
    }

    public static IQueryable<T> IncludeMany<T>(this IQueryable<T> queryable,
        IEnumerable<Expression<Func<T, object>>> includeExpression) where T : class
    {
        return includeExpression.Aggregate(seed: queryable,
            func: (current, include) => current.IncludeIfNotNull(includeExpression: include));
    }

    private static IQueryable<T> IncludeIfNotNull<T>(this IQueryable<T> queryable,
        Expression<Func<T, object>>? includeExpression) where T : class
    {
        return includeExpression is not null ? queryable.Include(navigationPropertyPath: includeExpression) : queryable;
    }
}