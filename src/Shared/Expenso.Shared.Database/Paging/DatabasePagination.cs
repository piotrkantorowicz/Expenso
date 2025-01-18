using Expenso.Shared.System.Types.Paging;
using Expenso.Shared.System.Types.Paging.Constants;

namespace Expenso.Shared.Database.Paging;

public sealed record DatabasePagination
{
    private DatabasePagination(int? page, int? limit)
    {
        Page = page is null or < 1 ? PaginationDefaults.Page : page.Value;

        Limit = limit switch
        {
            null or < 1 => PaginationDefaults.Limit,
            > PaginationDefaults.MaxLimit => PaginationDefaults.MaxLimit,
            _ => limit.Value
        };
    }

    public int Page { get; }

    public int Limit { get; }

    public static DatabasePagination New(int? page, int? limit)
    {
        return new DatabasePagination(page: page, limit: limit);
    }

    public static DatabasePagination New(Pagination? pagination)
    {
        return pagination is null ? Default : new DatabasePagination(page: pagination.Page, limit: pagination.Limit);
    }

    public static DatabasePagination Default => new(page: PaginationDefaults.Page, limit: PaginationDefaults.Limit);
}