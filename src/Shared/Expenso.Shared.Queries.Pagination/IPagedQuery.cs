namespace Expenso.Shared.Queries.Pagination;

public interface IPagedQuery : IQuery
{
    System.Types.Paging.Pagination? Pagination { get; }
}

public interface IPagedQuery<T> : IPagedQuery, IQuery<T>;