namespace Expenso.Shared.System.Types.Pagination;

public sealed record Paging
{
    public Paging(int page, int limit)
    {
        Page = page;
        Limit = limit;
    }

    public int Page { get; set; }

    public int Limit { get; set; }

    public static Paging Default => new(page: 1, limit: 10);

    public static bool TryParse(string? value, out Paging? paging)
    {
        paging = null;

        if (string.IsNullOrEmpty(value: value))
        {
            return false;
        }

        string[]? parts = value.Split(separator: ',');

        if (parts.Length != 2 || !int.TryParse(s: parts[0], result: out int page) ||
            !int.TryParse(s: parts[1], result: out int limit))
        {
            return false;
        }

        paging = new Paging(page: page, limit: limit);

        return true;
    }
}