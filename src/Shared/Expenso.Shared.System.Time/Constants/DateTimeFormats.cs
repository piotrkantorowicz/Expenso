namespace Expenso.Shared.System.Time.Constants;

public static class DateTimeFormats
{
    public static readonly string[] SupportedFormats = [Iso8601, Iso8601TimeZone];
    public static readonly string[] SupportedDateTimeFormats = [Iso8601];
    public static readonly string[] SupportedDateTimeOffsetFormats = [Iso8601TimeZone];
    public const string Iso8601TimeZone = "o"; // yyyy-MM-ddTHH:mm:ss.fffffffzzz
    public const string Iso8601 = "s"; // yyyy-MM-ddTHH:mm:ss
}