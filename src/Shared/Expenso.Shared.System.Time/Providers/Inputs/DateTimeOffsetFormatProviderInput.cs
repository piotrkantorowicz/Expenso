namespace Expenso.Shared.System.Time.Providers.Inputs;

public static class DateTimeOffsetFormatConstants
{
    public const string DefaultHeadername = "Date-Time-Offset-Format";
    public const string DefaultHeaderPrefix = "Date-Time-Offset-Format=";
    public const string DefaultCookiePrefix = "dateTimeOffsetFormat=";
    public const string DefaultCookieName = ".AspNetCore.DateTimeOffsetFormat";
    public const string DefaultQueryStringKey = "DateTimeOffsetFormat";
    public const string DefaultQueryStringPrefix = "DateTimeOffsetFormat=";
}

public record DateTimeOffsetFormatProviderInput : IProviderInput
{
    private DateTimeOffsetFormatProviderInput(string key, string prefix)
    {
        Key = key;
        Prefix = prefix;
    }

    private static DateTimeOffsetFormatProviderInput HeaderInput => new(
        key: DateTimeOffsetFormatConstants.DefaultHeadername,
        prefix: DateTimeOffsetFormatConstants.DefaultHeaderPrefix);

    private static DateTimeOffsetFormatProviderInput CookieInput => new(
        key: DateTimeOffsetFormatConstants.DefaultCookieName,
        prefix: DateTimeOffsetFormatConstants.DefaultCookiePrefix);

    private static DateTimeOffsetFormatProviderInput QueryStringInput =>
        new(key: DateTimeOffsetFormatConstants.DefaultQueryStringKey,
            prefix: DateTimeOffsetFormatConstants.DefaultQueryStringPrefix);

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