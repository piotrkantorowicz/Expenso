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
        int? parsedPage = null;

        if (parts.Length > 0 && int.TryParse(s: parts[0], result: out int page))
        {
            parsedPage = page >= 1 ? page : null;
        }

        int? parsedLimit = null;

        if (parts.Length > 1 && int.TryParse(s: parts[1], result: out int limit))
        {
            parsedLimit = limit is >= 1 and <= PaginationDefaults.MaxLimit ? limit : null;
        }

        paging = new Pagination(Page: parsedPage ?? PaginationDefaults.Page,
            Limit: parsedLimit ?? PaginationDefaults.Limit);

        return true;
    }
}