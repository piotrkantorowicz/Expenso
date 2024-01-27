using System.Linq.Expressions;

using Microsoft.EntityFrameworkCore;

namespace Expenso.Shared.Database.EfCore.Extensions;

public static class QueryableExtensions
{
    public static IQueryable<T> Tracking<T>(this IQueryable<T> queryable, bool useTracking) where T : class
    {
        return useTracking ? queryable : queryable.AsNoTracking();
    }

    public static IQueryable<T> Include<T>(this IQueryable<T> queryable, bool include,
        Expression<Func<T, object>> includeExpression) where T : class
    {
        return include ? queryable.Include(includeExpression) : queryable;
    }
}