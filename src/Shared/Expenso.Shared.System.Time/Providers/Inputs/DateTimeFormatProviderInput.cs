namespace Expenso.Shared.System.Time.Providers.Inputs;

public static class DateTimeFormatConstants
{
    public const string DefaultHeadername = "Date-Time-Format";
    public const string DefaultHeaderPrefix = "Date-Time-Format=";
    public const string DefaultCookiePrefix = "dateTimeFormat=";
    public const string DefaultCookieName = ".AspNetCore.DateTimeFormat";
    public const string DefaultQueryStringKey = "DateTimeFormat";
    public const string DefaultQueryStringPrefix = "DateTimeFormat=";
}

public record DateTimeFormatProviderInput : IProviderInput
{
    private DateTimeFormatProviderInput(string key, string prefix)
    {
        Key = key;
        Prefix = prefix;
    }

    private static DateTimeFormatProviderInput HeaderInput => new(key: DateTimeFormatConstants.DefaultHeadername,
        prefix: DateTimeFormatConstants.DefaultHeaderPrefix);

    private static DateTimeFormatProviderInput CookieInput => new(key: DateTimeFormatConstants.DefaultCookieName,
        prefix: DateTimeFormatConstants.DefaultCookiePrefix);

    private static DateTimeFormatProviderInput QueryStringInput =>
        new(key: DateTimeFormatConstants.DefaultQueryStringKey,
            prefix: DateTimeFormatConstants.DefaultQueryStringPrefix);

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
}