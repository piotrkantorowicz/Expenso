namespace Expenso.Shared.System.Types.Pagination;

public sealed record PagedList<T> : IPagedList<T>
{
    // Required to be public for serialization
    // ReSharper disable once MemberCanBePrivate.Global
    public PagedList() : this(items: Array.Empty<T>(), currentPage: 1, resultsPerPage: 1, totalPages: 10,
        totalResults: 0)
    {
    }

    private PagedList(IReadOnlyCollection<T> items, int currentPage, int resultsPerPage, int totalPages,
        long totalResults)
    {
        CurrentPage = currentPage;
        ResultsPerPage = resultsPerPage;
        TotalPages = totalPages;
        TotalResults = totalResults;
        Items = items;
    }

    public int CurrentPage { get; }

    public int ResultsPerPage { get; }

    public int TotalPages { get; }

    public long TotalResults { get; }

    public IReadOnlyCollection<T> Items { get; }

    public bool Empty => Items.Count == 0;

    public static IPagedList<T> Create(IReadOnlyCollection<T> items, int currentPage, int resultsPerPage,
        int totalPages, long totalResults)
    {
        return new PagedList<T>(items: items, currentPage: currentPage, resultsPerPage: resultsPerPage,
            totalPages: totalPages, totalResults: totalResults);
    }

    public static IPagedList<T> From<TResult>(IPagedList<TResult> result, IReadOnlyCollection<T> items)
    {
        return new PagedList<T>(items: items, currentPage: result.CurrentPage, resultsPerPage: result.ResultsPerPage,
            totalPages: result.TotalPages, totalResults: result.TotalResults);
    }

    public static IPagedList<T> AsEmpty => new PagedList<T>();

    public IPagedList<TResult> Map<TResult>(Func<T, TResult> map)
    {
        return PagedList<TResult>.From(result: this, items: Items.Select(selector: map).ToList());
    }
}