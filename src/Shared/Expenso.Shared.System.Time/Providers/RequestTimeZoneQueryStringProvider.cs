using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace Expenso.Shared.System.Time.Providers;

public sealed class RequestTimeZoneQueryStringProvider : RequestTimeZoneProvider
{
    public RequestTimeZoneQueryStringProvider(string? queryStringKey = null)
    {
        QueryStringKey = queryStringKey ?? "time-zone";
    }

    public string QueryStringKey { get; }

    public override Task<ProviderTimeZoneResult> DetermineProviderTimeZoneResult(HttpContext httpContext)
    {
        ArgumentNullException.ThrowIfNull(argument: httpContext);

        if (!httpContext.Request.Query.TryGetValue(key: QueryStringKey, value: out StringValues values))
        {
            return DefaultProviderTimeZoneResult;
        }

        string? value = values.FirstOrDefault(predicate: v => !string.IsNullOrEmpty(value: v));

        return CreateResult(value: value);
    }
}