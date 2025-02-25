namespace Expenso.Shared.System.Time.Providers.Inputs;

public static class TimeZoneConstants
{
    public const string DefaultHeadername = "Time-Zone";
    public const string DefaultHeaderPrefix = "Time-Zone=";
    public const string DefaultCookiePrefix = "timezone=";
    public const string DefaultCookieName = ".AspNetCore.TimeZone";
    public const string DefaultQueryStringKey = "TimeZone";
    public const string DefaultQueryStringPrefix = "TimeZone=";
}

public record TimeZoneProviderInput : IProviderInput
{
    private TimeZoneProviderInput(string key, string prefix)
    {
        Key = key;
        Prefix = prefix;
    }

    private static TimeZoneProviderInput HeaderInput => new(key: TimeZoneConstants.DefaultHeadername,
        prefix: TimeZoneConstants.DefaultHeaderPrefix);

    private static TimeZoneProviderInput CookieInput => new(key: TimeZoneConstants.DefaultCookieName,
        prefix: TimeZoneConstants.DefaultCookiePrefix);

    private static TimeZoneProviderInput QueryStringInput =>
        new(key: TimeZoneConstants.DefaultQueryStringKey, prefix: TimeZoneConstants.DefaultQueryStringPrefix);

    public string Key { get; }

    public string Prefix { get; }

    public IProviderInput GetInput(RequestProviderType requestProviderType)
    {
        return requestProviderType switch
        {
            RequestProviderType.Header => HeaderInput,
            RequestProviderType.Cookie => CookieInput,
            RequestProviderType.QueryString => QueryStringInput,
            _ => HeaderInput
        };
    }

    // public static TimeZoneProviderInput GetInput(RequestProviderType requestProviderType)
    // {
    //     return requestProviderType switch
    //     {
    //         RequestProviderType.Header => HeaderInput,
    //         RequestProviderType.Cookie => CookieInput,
    //         RequestProviderType.QueryString => QueryStringInput,
    //         _ => HeaderInput
    //     };
    // }
}