using Microsoft.AspNetCore.Http;

namespace Expenso.Shared.System.Time.Providers;

public sealed class RequestTimeZoneCookieProvider : RequestTimeZoneProvider
{
    private const string Prefix = "timezone=";
    private const string DefaultCookieName = ".AspNetCore.TimeZone";

    public RequestTimeZoneCookieProvider(string? cookieName = null)
    {
        CookieName = cookieName ?? DefaultCookieName;
    }

    public string CookieName { get; }

    public override Task<ProviderTimeZoneResult> DetermineProviderTimeZoneResult(HttpContext httpContext)
    {
        ArgumentNullException.ThrowIfNull(argument: httpContext);

        return httpContext.Request.Cookies.TryGetValue(key: CookieName, value: out string? value)
            ? CreateResult(value: value, trimPrefix: true, prefix: Prefix)
            : DefaultProviderTimeZoneResult;
    }
}