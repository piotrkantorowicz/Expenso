using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace Expenso.Shared.System.Time.Providers;

public sealed class RequestTimeZoneHeaderProvider : RequestTimeZoneProvider
{
    public RequestTimeZoneHeaderProvider(string? headerkey = null)
    {
        Headerkey = headerkey ?? "time-zone";
    }

    public string Headerkey { get; }

    public override Task<ProviderTimeZoneResult> DetermineProviderTimeZoneResult(HttpContext httpContext)
    {
        ArgumentNullException.ThrowIfNull(argument: httpContext);

        if (!httpContext.Request.Headers.TryGetValue(key: Headerkey, value: out StringValues values))
        {
            return DefaultProviderTimeZoneResult;
        }

        string? value = values.FirstOrDefault(predicate: v => !string.IsNullOrEmpty(value: v));

        return CreateResult(value: value);
    }
}