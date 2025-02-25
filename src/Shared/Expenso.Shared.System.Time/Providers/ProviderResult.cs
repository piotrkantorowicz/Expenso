namespace Expenso.Shared.System.Time.Providers;

public sealed record ProviderResult
{
    private ProviderResult(string? value)
    {
        Value = value;
    }

    public static ProviderResult Null => new(value: null);

    public string? Value { get; }

    public static ProviderResult New(string value)
    {
        return new ProviderResult(value: value);
    }
}