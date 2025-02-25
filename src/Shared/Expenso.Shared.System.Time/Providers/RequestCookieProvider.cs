using Expenso.Shared.System.Time.Providers.Inputs;

using Microsoft.AspNetCore.Http;

namespace Expenso.Shared.System.Time.Providers;

public sealed class RequestCookieProvider : RequestProvider
{
    public string? CookieName { get; private set; }

    public override RequestProviderType ProviderType => RequestProviderType.Cookie;

    public override Task<ProviderResult> DetermineProviderResult(HttpContext httpContext, IProviderInput providerInput)
    {
        ArgumentNullException.ThrowIfNull(argument: httpContext);
        ArgumentNullException.ThrowIfNull(argument: providerInput);
        CookieName ??= providerInput.Key;

        return httpContext.Request.Cookies.TryGetValue(key: CookieName, value: out string? value)
            ? CreateResult(value: value, trimPrefix: true, prefix: providerInput.Prefix)
            : DefaultProviderTimeZoneResult;
    }
}