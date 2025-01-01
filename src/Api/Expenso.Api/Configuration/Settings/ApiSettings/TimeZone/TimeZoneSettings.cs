using Expenso.Shared.System.Configuration.Settings;
using Expenso.Shared.System.Time.Providers;
using Expenso.Shared.System.Time.Providers.Interfaces;

namespace Expenso.Api.Configuration.Settings.ApiSettings.TimeZone;

internal sealed record TimeZoneSettings : ISettings
{
    public string? Id { get; init; }

    public bool? EnableRequestToUtc { get; init; }

    public bool? EnableResponseToLocal { get; init; }

    public string[]? SupportedDateTimeFormats { get; init; }

    public string[]? SupportedDateTimeOffsetFormats { get; init; }

    public TimeZoneProviderType TimeZoneProviderType { get; init; }

    public static IList<IRequestTimeZoneProvider>? GetRequestTimeZoneProviders(
        TimeZoneProviderType timeZoneProviderType)
    {
        return timeZoneProviderType switch
        {
            TimeZoneProviderType.QueryString => new List<IRequestTimeZoneProvider>
            {
                new RequestTimeZoneQueryStringProvider()
            },
            TimeZoneProviderType.Header => new List<IRequestTimeZoneProvider>
            {
                new RequestTimeZoneHeaderProvider()
            },
            TimeZoneProviderType.Cookie => new List<IRequestTimeZoneProvider>
            {
                new RequestTimeZoneCookieProvider()
            },
            TimeZoneProviderType.All => new List<IRequestTimeZoneProvider>
            {
                new RequestTimeZoneQueryStringProvider(),
                new RequestTimeZoneHeaderProvider(),
                new RequestTimeZoneCookieProvider()
            },
            _ => null
        };
    }
}