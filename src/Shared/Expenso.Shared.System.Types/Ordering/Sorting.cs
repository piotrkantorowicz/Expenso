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

        List<ISorter>? sorters = new();
        string[]? parts = value.Split(separator: ',');

        foreach (string? part in parts)
        {
            string[]? sorterParts = part.Split(separator: ':');

            if (sorterParts.Length != 2 ||
                !Enum.TryParse(value: sorterParts[1], ignoreCase: true, result: out SortOrder sortOrder))
            {
                return false;
            }

            sorters.Add(item: new Sorter(SortBy: sorterParts[0], SortOrder: sortOrder));
        }

        sorting = new Sorting(sorters: sorters);

        return true;
    }
}