using Expenso.Shared.System.Time.Request.Settings;

using Microsoft.AspNetCore.Http;

namespace Expenso.Shared.System.Time.Providers;

internal sealed class RequestTimeZoneCookieProvider : RequestTimeZoneProvider
{
    private const string Prefix = "timezone=";
    private const string DefaultCookieName = ".AspNetCore.TimeZone";

    public RequestTimeZoneCookieProvider(RequestTimeZoneOptions? options, string? cookieName = null) : base(
        options: options)
    {
        CookieName = cookieName ?? DefaultCookieName;
    }

    public string CookieName { get; }

    public override Task<ProviderTimeZoneResult> DetermineProviderTimeZoneResult(HttpContext httpContext)
    {
        string? value = httpContext.Request.Cookies[key: CookieName];

        return ValidateAndCreateResult(value: value, trimPrefix: true, prefix: Prefix);
    }
}