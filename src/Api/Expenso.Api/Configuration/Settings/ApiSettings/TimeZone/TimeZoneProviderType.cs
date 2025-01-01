namespace Expenso.Api.Configuration.Settings.ApiSettings.TimeZone;

[Flags]
internal enum TimeZoneProviderType
{
    None = 0,
    QueryString = 1,
    Header = 2,
    Cookie = 4,
    All = QueryString | Header | Cookie
}