using Expenso.Shared.System.Types.Pagination;
using Expenso.Shared.System.Types.Pagination.Constants;

namespace Expenso.Shared.System.Types.Collections;

public static class EnumerableExtensions
{
    public static IPagedList<T> Pagination<T>(this IEnumerable<T> enumerable, Paging? pagination) where T : class
    {
        IReadOnlyCollection<T> enumerableAsList = enumerable.ToList().AsReadOnly();
        int page = pagination?.Page ?? PaginationDefaults.Page;
        int limit = pagination?.Limit ?? PaginationDefaults.Limit;
        int allRecordsCount = enumerableAsList.Count;

        IReadOnlyCollection<T> budgetPermissionRequests =
            enumerableAsList.Skip(count: limit * (page - 1)).Take(count: limit).ToList();

        int totalPages = allRecordsCount / limit;

        return PagedList<T>.Create(items: budgetPermissionRequests, currentPage: page, resultsPerPage: limit,
            totalPages: totalPages is 0 ? 1 : totalPages, totalResults: allRecordsCount);
    }
}