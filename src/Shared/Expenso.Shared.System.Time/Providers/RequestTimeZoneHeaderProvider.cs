using Expenso.Shared.System.Time.Request;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace Expenso.Shared.System.Time.Providers;

internal sealed class RequestTimeZoneHeaderProvider : RequestTimeZoneProvider
{
    public RequestTimeZoneHeaderProvider(RequestTimeZoneOptions? options, string? headerkey = null) : base(
        options: options)
    {
        Headerkey = headerkey ?? "timezone";
    }

    public string Headerkey { get; }

    public override Task<ProviderTimeZoneResult?> DetermineProviderTimeZoneResult(HttpContext httpContext)
    {
        ArgumentNullException.ThrowIfNull(argument: httpContext);
        StringValues value = httpContext.Request.Headers[key: Headerkey];

        if (string.IsNullOrEmpty(value: value))
        {
            return NullProviderTimeZoneResult;
        }

        ProviderTimeZoneResult? providerTimeZoneResult = new(name: value);

        return Task.FromResult(result: providerTimeZoneResult)!;
    }
}