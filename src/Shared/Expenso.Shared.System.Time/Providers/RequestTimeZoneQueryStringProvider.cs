using Expenso.Shared.System.Time.Request.Settings;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace Expenso.Shared.System.Time.Providers;

internal sealed class RequestTimeZoneQueryStringProvider : RequestTimeZoneProvider
{
    public RequestTimeZoneQueryStringProvider(RequestTimeZoneOptions? options, string? queryStringKey = null) : base(
        options: options)
    {
        QueryStringKey = queryStringKey ?? "time-zone";
    }

    public string QueryStringKey { get; }

    public override Task<ProviderTimeZoneResult> DetermineProviderTimeZoneResult(HttpContext httpContext)
    {
        if (!httpContext.Request.Query.TryGetValue(key: QueryStringKey, value: out StringValues values))
        {
            return DefaultProviderTimeZoneResult;
        }

        string? value = values.FirstOrDefault(predicate: v => !string.IsNullOrEmpty(value: v));

        return ValidateAndCreateResult(value: value);
    }
}