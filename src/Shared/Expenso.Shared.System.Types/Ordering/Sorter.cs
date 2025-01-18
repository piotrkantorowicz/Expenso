namespace Expenso.Shared.System.Types.Ordering;

public sealed record Sorter(string SortBy, SortOrder SortOrder) : ISorter;