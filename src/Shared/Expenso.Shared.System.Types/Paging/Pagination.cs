using Expenso.Shared.System.Types.Paging.Constants;

namespace Expenso.Shared.System.Types.Paging;

public sealed record Pagination(int? Page, int? Limit)
{
    private Pagination() : this(Page: PaginationDefaults.Page, Limit: PaginationDefaults.Limit)
    {
    }

    public static bool TryParse(string? value, out Pagination? paging)
    {
        paging = null;

        if (string.IsNullOrEmpty(value: value))
        {
            return false;
        }

        int? parsedPage, parsedLimit;
        string[] parts = value.Split(separator: ',');

        if (parts.Length > 0 && int.TryParse(s: parts[0], result: out int page))
        {
            if (page >= 1)
            {
                parsedPage = page;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }

        if (parts.Length > 1 && int.TryParse(s: parts[1], result: out int limit))
        {
            if (limit is >= 1 and <= PaginationDefaults.MaxLimit)
            {
                parsedLimit = limit;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }

        paging = new Pagination(Page: parsedPage ?? PaginationDefaults.Page,
            Limit: parsedLimit ?? PaginationDefaults.Limit);

        return true;
    }
}