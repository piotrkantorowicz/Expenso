using Expenso.Shared.System.Time.Constants;
using Expenso.Shared.System.Time.Providers;
using Expenso.Shared.System.Time.Providers.Interfaces;

namespace Expenso.Shared.System.Time.Request.Settings;

public sealed record RequestTimeZoneOptions
{
    private readonly RequestTimeZone _defaultRequestTimeZone = new(name: TimeZoneIds.Utc);

    public RequestTimeZoneOptions()
    {
        DatesFormat = DateTimeFormats.Iso8601;
        MvcOptionType = MvcOptionType.All;

        RequestTimeZoneProviders = new List<IRequestTimeZoneProvider>
        {
            new RequestTimeZoneQueryStringProvider(options: this),
            new RequestTimeZoneHeaderProvider(options: this),
            new RequestTimeZoneCookieProvider(options: this)
        };
    }

    public RequestTimeZone DefaultRequestTimeZone => string.IsNullOrEmpty(value: Id)
        ? _defaultRequestTimeZone
        : new RequestTimeZone(name: Id);

    public string? Id { get; set; }

    public bool EnableRequestToUtc { get; set; }

    public bool EnableResponseToLocal { get; set; }

    public MvcOptionType MvcOptionType { get; set; }

    public string DatesFormat { get; set; }

    public IList<IRequestTimeZoneProvider>? RequestTimeZoneProviders { get; set; }

    internal string GetDefaultHeaderName()
    {
        RequestTimeZoneHeaderProvider? headerProvider =
            RequestTimeZoneProviders?.OfType<RequestTimeZoneHeaderProvider>().FirstOrDefault();

        return headerProvider is null ? "time-zone" : headerProvider.Headerkey;
    }

    internal string GetDefaultCookieName()
    {
        RequestTimeZoneCookieProvider? cookieProvider =
            RequestTimeZoneProviders?.OfType<RequestTimeZoneCookieProvider>().FirstOrDefault();

        return cookieProvider is null ? "time-zone" : cookieProvider.CookieName;
    }

    internal string GetDefaultQueryName()
    {
        RequestTimeZoneQueryStringProvider? queryProvider =
            RequestTimeZoneProviders?.OfType<RequestTimeZoneQueryStringProvider>().FirstOrDefault();

        return queryProvider is null ? "time-zone" : queryProvider.QueryStringKey;
    }
}