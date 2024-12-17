namespace Expenso.Shared.System.Time.Providers;

public sealed class ProviderTimeZoneResult
{
    public ProviderTimeZoneResult(string? name)
    {
        TimeZoneName = name ?? throw new ArgumentNullException(paramName: nameof(name));
    }

    public string TimeZoneName { get; }
}