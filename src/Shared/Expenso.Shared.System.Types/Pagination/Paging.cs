using Expenso.Shared.System.Types.Pagination.Constants;

namespace Expenso.Shared.System.Types.Pagination;

public sealed record Paging
{
    public Paging(int? page, int? limit)
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

    public static Paging Default => new(page: PaginationDefaults.Page, limit: PaginationDefaults.Limit);

    public static bool TryParse(string? value, out Paging? paging)
    {
        paging = null;

        if (string.IsNullOrEmpty(value: value))
        {
            return false;
        }

        string[] parts = value.Split(separator: ',');

        if (parts.Length != 2 || !int.TryParse(s: parts[0], result: out int page) ||
            !int.TryParse(s: parts[1], result: out int limit))
        {
            paging = Default;
        }
        else
        {
            paging = new Paging(page: page, limit: limit);
        }

        return true;
    }
}