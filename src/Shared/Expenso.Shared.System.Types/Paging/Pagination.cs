using Expenso.Shared.System.Types.Paging.Constants;

namespace Expenso.Shared.System.Types.Paging;

public sealed record Pagination(int? Page, int? Limit)
{
    private Pagination() : this(Page: PaginationDefaults.Page, Limit: PaginationDefaults.Limit)
    {
    }

    public static bool TryParse(string? value, out Pagination? paging)
    {
        paging = new Pagination();

        if (string.IsNullOrEmpty(value: value))
        {
            return true;
        }

        string[] parts = value.Split(separator: ',');

        if (parts.Length != 2 || !int.TryParse(s: parts[0], result: out int page) ||
            !int.TryParse(s: parts[1], result: out int limit))
        {
            return true;
        }

        if (page < 1 || limit < 1 || limit > PaginationDefaults.MaxLimit)
        {
            return true;
        }

        paging = new Pagination(Page: page, Limit: limit);

        return true;
    }
}