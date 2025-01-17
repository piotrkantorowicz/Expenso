using System.Linq.Expressions;

using Expenso.Shared.System.Types.Ordering;
using Expenso.Shared.System.Types.Ordering.Constants;
using Expenso.Shared.System.Types.Pagination;
using Expenso.Shared.System.Types.Pagination.Constants;

using Microsoft.EntityFrameworkCore;

namespace Expenso.Shared.Database.EfCore.Collections;

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

    public static IQueryable<T> ApplySorting<T>(this IQueryable<T> source, Sorting? sorting)
    {
        ArgumentNullException.ThrowIfNull(argument: source);

        if (sorting?.Sorters is null or { Count: 0 })
        {
            return source;
        }

        ParameterExpression? parameter = Expression.Parameter(type: typeof(T), name: "item");
        Expression? sortExpression = null;

        foreach (ISorter sorter in sorting.Sorters)
        {
            if (string.IsNullOrWhiteSpace(value: sorter.SortBy))
            {
                continue;
            }

            MemberExpression? property = GetPropertyExpression(parameter: parameter, propertyPath: sorter.SortBy);

            if (property == null)
            {
                continue;
            }

            LambdaExpression? lambda = Expression.Lambda(body: property, parameter);
            string? methodName = GetSortMethodName(sortOrder: sorter.SortOrder, isFirstSort: sortExpression is null);

            MethodCallExpression? orderByCall = Expression.Call(type: typeof(Queryable), methodName: methodName,
                typeArguments:
                [
                    typeof(T),
                    property.Type
                ], sortExpression ?? source.Expression, Expression.Quote(expression: lambda));

            sortExpression = orderByCall;
        }

        return sortExpression is null ? source : source.Provider.CreateQuery<T>(expression: sortExpression);
    }

    private static MemberExpression? GetPropertyExpression(ParameterExpression parameter, string propertyPath)
    {
        string[]? properties = propertyPath.Split(separator: '.');
        Expression? property = null;

        foreach (string? prop in properties)
        {
            try
            {
                property = Expression.Property(expression: parameter, propertyName: prop);
            }
            catch
            {
                property = null;
            }
        }

        return property as MemberExpression;
    }

    private static string GetSortMethodName(SortOrder sortOrder, bool isFirstSort)
    {
        return (sortOrder, isFirstSort) switch
        {
            (SortOrder.Descending, true) => OrderingTypes.OrderByDescending,
            (SortOrder.Descending, false) => OrderingTypes.ThenByDescending,
            (SortOrder.Ascending, true) => OrderingTypes.OrderBy,
            (SortOrder.Ascending, false) => OrderingTypes.ThenBy,
            (SortOrder.None, true) => OrderingTypes.OrderBy,
            (SortOrder.None, false) => OrderingTypes.ThenBy,
            _ => throw new ArgumentOutOfRangeException(paramName: nameof(SortOrder), actualValue: sortOrder,
                message: string.Empty)
        };
    }

    private static IQueryable<T> IncludeIfNotNull<T>(this IQueryable<T> queryable,
        Expression<Func<T, object>>? includeExpression) where T : class
    {
        return includeExpression is not null ? queryable.Include(navigationPropertyPath: includeExpression) : queryable;
    }
}