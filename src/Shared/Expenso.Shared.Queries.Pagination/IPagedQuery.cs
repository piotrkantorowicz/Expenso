using Expenso.Shared.System.Types.Pagination;

namespace Expenso.Shared.Queries.Pagination;

public interface IPagedQuery : IQuery
{
    Paging? Pagination { get; }
}

public interface IPagedQuery<T> : IPagedQuery, IQuery<T>;