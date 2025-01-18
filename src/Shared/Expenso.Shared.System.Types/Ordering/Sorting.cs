namespace Expenso.Shared.System.Types.Ordering;

public sealed record Sorting
{
    private Sorting(IEnumerable<ISorter>? sorters)
    {
        Sorters = sorters?.ToList() ?? [];
    }

    public ICollection<ISorter> Sorters { get; }

    public static Sorting New(IEnumerable<ISorter>? sorters)
    {
        return new Sorting(sorters: sorters);
    }

    public static Sorting Default => new(sorters: []);

    public static bool TryParse(string? value, out Sorting? sorting)
    {
        sorting = null;

        if (string.IsNullOrEmpty(value: value))
        {
            return false;
        }

        List<ISorter> sorters = [];
        string[] parts = value.Split(separator: ',', options: StringSplitOptions.RemoveEmptyEntries);

        foreach (string? part in parts)
        {
            string[] sorterParts = part.Trim().Split(separator: ':', options: StringSplitOptions.RemoveEmptyEntries);

            if (sorterParts.Length != 2 ||
                !Enum.TryParse(value: sorterParts[1], ignoreCase: true, result: out SortOrder sortOrder))
            {
                return false;
            }

            string sortBy = sorterParts[0].Trim();

            if (string.IsNullOrEmpty(value: sortBy))
            {
                return false;
            }

            sorters.Add(item: new Sorter(SortBy: sortBy, SortOrder: sortOrder));
        }

        if (sorters.Count == 0)
        {
            return false;
        }

        sorting = new Sorting(sorters: sorters);

        return true;
    }
}