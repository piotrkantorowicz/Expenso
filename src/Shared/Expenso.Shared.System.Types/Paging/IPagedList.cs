namespace Expenso.Shared.System.Types.Paging;

public interface IPagedList
{
    public int CurrentPage { get; }

    public int ResultsPerPage { get; }

    public int TotalPages { get; }

    public long TotalResults { get; }
}

public interface IPagedList<out T> : IPagedList
{
    public IReadOnlyCollection<T> Items { get; }

    IPagedList<TResult> Map<TResult>(Func<T, TResult> map);
}