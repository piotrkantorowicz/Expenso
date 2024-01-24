using Microsoft.EntityFrameworkCore;

namespace Expenso.Shared.Database.EfCore.Extensions;

public static class DbSetExtensions
{
    public static IQueryable<T> Tracking<T>(this DbSet<T> dbSet, bool useTracking) where T : class
    {
        return useTracking ? dbSet : dbSet.AsNoTracking();
    }
}