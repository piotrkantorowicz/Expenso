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

    public static IList<IRequestProvider>? GetRequestTimeZoneProviders(TimeZoneProviderType timeZoneProviderType)
    {
        return timeZoneProviderType switch
        {
            TimeZoneProviderType.QueryString => new List<IRequestProvider>
            {
                new RequestQueryStringProvider()
            },
            TimeZoneProviderType.Header => new List<IRequestProvider>
            {
                new RequestHeaderProvider()
            },
            TimeZoneProviderType.Cookie => new List<IRequestProvider>
            {
                new RequestCookieProvider()
            },
            TimeZoneProviderType.All => new List<IRequestProvider>
            {
                new RequestQueryStringProvider(),
                new RequestHeaderProvider(),
                new RequestCookieProvider()
            },
            _ => null
        };
    }
}