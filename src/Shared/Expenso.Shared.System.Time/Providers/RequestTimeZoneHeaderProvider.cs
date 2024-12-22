using Expenso.Shared.System.Time.Request.Settings;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace Expenso.Shared.System.Time.Providers;

internal sealed class RequestTimeZoneHeaderProvider : RequestTimeZoneProvider
{
    public RequestTimeZoneHeaderProvider(RequestTimeZoneOptions? options, string? headerkey = null) : base(
        options: options)
    {
        Headerkey = headerkey ?? "time-zone";
    }

    public string Headerkey { get; }

    public override Task<ProviderTimeZoneResult> DetermineProviderTimeZoneResult(HttpContext httpContext)
    {
        if (!httpContext.Request.Headers.TryGetValue(key: Headerkey, value: out StringValues values))
        {
            return DefaultProviderTimeZoneResult;
        }

        string? value = values.FirstOrDefault(predicate: v => !string.IsNullOrEmpty(value: v));

        return ValidateAndCreateResult(value: value);
    }
}