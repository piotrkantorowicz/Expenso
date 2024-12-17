using Expenso.Shared.System.Time.Request;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace Expenso.Shared.System.Time.Providers;

internal sealed class RequestTimeZoneQueryStringProvider : RequestTimeZoneProvider
{
    public RequestTimeZoneQueryStringProvider(RequestTimeZoneOptions? options, string? queryStringKey = null) : base(
        options: options)
    {
        QueryStringKey = queryStringKey ?? "timezone";
    }

    public string QueryStringKey { get; }

    public override Task<ProviderTimeZoneResult?> DetermineProviderTimeZoneResult(HttpContext httpContext)
    {
        ArgumentNullException.ThrowIfNull(argument: httpContext);
        StringValues value = httpContext.Request.Query[key: QueryStringKey];

        if (string.IsNullOrEmpty(value: value))
        {
            return NullProviderTimeZoneResult;
        }

        ProviderTimeZoneResult? providerTimeZoneResult = new(name: value);

        return Task.FromResult(result: providerTimeZoneResult)!;
    }
}