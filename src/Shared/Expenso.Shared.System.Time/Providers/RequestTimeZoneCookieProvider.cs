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
        string? value = httpContext.Request.Cookies[key: CookieName];

        return CreateResult(value: value, trimPrefix: true, prefix: Prefix);
    }
}