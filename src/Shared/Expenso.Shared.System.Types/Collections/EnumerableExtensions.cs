using Expenso.Shared.System.Types.Paging;
using Expenso.Shared.System.Types.Paging.Constants;

namespace Expenso.Shared.System.Types.Collections;

public static class EnumerableExtensions
{
    public static IPagedList<T> Pagination<T>(this IEnumerable<T> enumerable, Pagination? pagination) where T : class
    {
        IReadOnlyCollection<T> enumerableAsList = enumerable.ToList();
        int page = Math.Max(val1: 1, val2: pagination?.Page ?? PaginationDefaults.Page);
        int limit = Math.Max(val1: 1, val2: pagination?.Limit ?? PaginationDefaults.Limit);
        int allRecordsCount = enumerableAsList.Count;
        int maxPage = (int)Math.Ceiling(a: allRecordsCount / (double)limit);
        page = Math.Min(val1: page, val2: maxPage);

        IReadOnlyCollection<T> budgetPermissionRequests =
            enumerableAsList.Skip(count: limit * (page - 1)).Take(count: limit).ToList();

        int totalPages = (int)Math.Ceiling(a: allRecordsCount / (double)limit);
        int currentPage = Math.Min(val1: totalPages, val2: page);

        return PagedList<T>.Create(items: budgetPermissionRequests, currentPage: currentPage, resultsPerPage: limit,
            totalPages: Math.Max(val1: 1, val2: totalPages), totalResults: allRecordsCount);
    }
}