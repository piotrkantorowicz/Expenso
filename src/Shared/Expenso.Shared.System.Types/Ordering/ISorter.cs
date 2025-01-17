namespace Expenso.Shared.System.Types.Ordering;

public interface ISorter
{
    public string SortBy { get; }

    public SortOrder SortOrder { get; }
}