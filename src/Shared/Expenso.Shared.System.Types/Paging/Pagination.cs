namespace Expenso.Shared.System.Types.Paging;

public sealed record Pagination(int? Page, int? Limit)
{
    private Pagination() : this(Page: null, Limit: null)
    {
    }

    public static bool TryParse(string? value, out Pagination? paging)
    {
        paging = null;

        if (string.IsNullOrEmpty(value: value))
        {
            return false;
        }

        string[] parts = value.Split(separator: ',');

        paging = parts.Length != 2 || !int.TryParse(s: parts[0], result: out int page) ||
                 !int.TryParse(s: parts[1], result: out int limit)
            ? new Pagination()
            : new Pagination(Page: page, Limit: limit);

        return true;
    }
}