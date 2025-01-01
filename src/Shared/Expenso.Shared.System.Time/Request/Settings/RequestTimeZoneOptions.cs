using Expenso.Shared.System.Time.Constants;
using Expenso.Shared.System.Time.Providers;
using Expenso.Shared.System.Time.Providers.Interfaces;

namespace Expenso.Shared.System.Time.Request.Settings;

public sealed record RequestTimeZoneOptions
{
    private readonly RequestTimeZone _defaultRequestTimeZone = new(name: TimeZoneIds.Utc);

    public RequestTimeZone DefaultRequestTimeZone => string.IsNullOrEmpty(value: Id)
        ? _defaultRequestTimeZone
        : new RequestTimeZone(name: Id);

    public string? Id { get; set; }

    public bool EnableRequestToUtc { get; set; }

    public bool EnableResponseToLocal { get; set; }

    public MvcOptionType MvcOptionType { get; set; } = MvcOptionType.All;

    public string[] SupportedDateTimeFormats { get; set; } = [DateTimeFormats.Iso8601];

    public string[] SupportedDateTimeOffsetFormats { get; set; } = [DateTimeFormats.Iso8601TimeZone];

    public IList<IRequestTimeZoneProvider> RequestTimeZoneProviders { get; set; } = new List<IRequestTimeZoneProvider>
    {
        new RequestTimeZoneQueryStringProvider(),
        new RequestTimeZoneHeaderProvider(),
        new RequestTimeZoneCookieProvider()
    };

    internal string GetDefaultHeaderName()
    {
        return GetDefaultValue<RequestTimeZoneHeaderProvider>(defaultValue: "time-zone",
            valueSelector: p => p.Headerkey);
    }

    internal string GetDefaultCookieName()
    {
        return GetDefaultValue<RequestTimeZoneCookieProvider>(defaultValue: "time-zone",
            valueSelector: p => p.CookieName);
    }

    internal string GetDefaultQueryName()
    {
        return GetDefaultValue<RequestTimeZoneQueryStringProvider>(defaultValue: "time-zone",
            valueSelector: p => p.QueryStringKey);
    }

    private string GetDefaultValue<T>(string defaultValue, Func<T, string> valueSelector)
        where T : IRequestTimeZoneProvider
    {
        T? provider = RequestTimeZoneProviders.OfType<T>().FirstOrDefault();

        return provider is null ? defaultValue : valueSelector(arg: provider);
    }
}