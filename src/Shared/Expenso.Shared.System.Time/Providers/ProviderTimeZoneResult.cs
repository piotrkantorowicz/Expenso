namespace Expenso.Shared.System.Time.Providers;

public sealed record ProviderTimeZoneResult
{
    private ProviderTimeZoneResult(string? name)
    {
        TimeZoneName = name;
    }

    public static ProviderTimeZoneResult New(string name)
    {
        return new ProviderTimeZoneResult(name: name);
    }

    public static ProviderTimeZoneResult Null => new(name: null);

    public string? TimeZoneName { get; }
}