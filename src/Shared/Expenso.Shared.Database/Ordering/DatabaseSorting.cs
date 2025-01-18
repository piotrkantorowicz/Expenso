using Expenso.Shared.System.Types.Ordering;

namespace Expenso.Shared.Database.Ordering;

public sealed record DatabaseSorting
{
    private DatabaseSorting(IEnumerable<DatabaseSorter>? sorters)
    {
        Sorters = sorters?.ToList() ?? [];
    }

    public ICollection<DatabaseSorter> Sorters { get; }

    public static DatabaseSorting New(IEnumerable<DatabaseSorter>? sorters)
    {
        return new DatabaseSorting(sorters: sorters);
    }

    public static DatabaseSorting New(Sorting? sorting)
    {
        return sorting is null
            ? Default
            : new DatabaseSorting(sorters: sorting.Sorters?.Select(selector: x =>
                new DatabaseSorter(SortBy: x.SortBy, SortOrder: (DatabaseSortOrder)x.SortOrder)));
    }

    public static DatabaseSorting Default => new(sorters: []);
}