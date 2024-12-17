using Expenso.Shared.System.Time.Request;

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

    public override Task<ProviderTimeZoneResult?> DetermineProviderTimeZoneResult(HttpContext httpContext)
    {
        ArgumentNullException.ThrowIfNull(argument: httpContext);
        string? value = httpContext.Request.Cookies[key: CookieName];

        if (string.IsNullOrEmpty(value: value))
        {
            return NullProviderTimeZoneResult;
        }

        ProviderTimeZoneResult? providerTimeZoneResult = new(name: value.Replace(oldValue: Prefix, newValue: ""));

        return Task.FromResult(result: providerTimeZoneResult)!;
    }
}