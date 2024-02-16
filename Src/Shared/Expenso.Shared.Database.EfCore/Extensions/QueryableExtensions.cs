using System.Linq.Expressions;

using Microsoft.EntityFrameworkCore;

namespace Expenso.Shared.Database.EfCore.Extensions;

public static class QueryableExtensions
{
    public static IQueryable<T> Tracking<T>(this IQueryable<T> queryable, bool? useTracking) where T : class
    {
        return useTracking == true ? queryable : queryable.AsNoTracking();
    }

    public static IQueryable<T> IncludeIf<T>(this IQueryable<T> queryable, bool include,
        Expression<Func<T, object>> includeExpression) where T : class
    {
        return include ? queryable.Include(includeExpression) : queryable;
    }

    public static IQueryable<T> IncludeIfNotNull<T>(this IQueryable<T> queryable,
        Expression<Func<T, object>>? includeExpression) where T : class
    {
        return includeExpression != null ? queryable.Include(includeExpression) : queryable;
    }

    public static IQueryable<T> IncludeMany<T>(this IQueryable<T> queryable,
        IEnumerable<Expression<Func<T, object>>> includeExpression) where T : class
    {
        return includeExpression.Aggregate(queryable, (current, include) => current.Include(include));
    }
}