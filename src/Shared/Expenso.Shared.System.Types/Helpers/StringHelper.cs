namespace Expenso.Shared.System.Types.Helpers;

public static class StringHelper
{
    public static string SafelyJoin(string? separator, params string?[]? values)
    {
        return values == null
            ? string.Empty
            : string
                .Join(separator: separator, values: values.Where(predicate: n => !string.IsNullOrWhiteSpace(value: n)))
                .Trim();
    }
}