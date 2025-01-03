using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace Expenso.Shared.System.Time.Providers;

public sealed class RequestTimeZoneHeaderProvider : RequestTimeZoneProvider
{
    private const string DefaultHeader = "Time-Zone";
    private const string Prefix = "Time-Zone=";

    public RequestTimeZoneHeaderProvider(string? headerkey = null)
    {
        Headerkey = headerkey ?? DefaultHeader;
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

        return CreateResult(value: value, trimPrefix: true, prefix: Prefix);
    }
}