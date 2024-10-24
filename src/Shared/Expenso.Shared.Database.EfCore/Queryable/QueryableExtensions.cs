using System.Linq.Expressions;

using Microsoft.EntityFrameworkCore;

namespace Expenso.Shared.Database.EfCore.Queryable;

public static class QueryableExtensions
{
    public static IQueryable<T> Tracking<T>(this IQueryable<T> queryable, bool? useTracking) where T : class
    {
        return useTracking is true ? queryable : queryable.AsNoTracking();
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