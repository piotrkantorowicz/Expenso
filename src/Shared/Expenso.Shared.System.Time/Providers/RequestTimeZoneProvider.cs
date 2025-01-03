using Expenso.Shared.System.Time.Providers.Interfaces;

using Microsoft.AspNetCore.Http;

namespace Expenso.Shared.System.Time.Providers;

public abstract class RequestTimeZoneProvider : IRequestTimeZoneProvider
{
    protected static readonly Task<ProviderTimeZoneResult> DefaultProviderTimeZoneResult =
        Task.FromResult(result: ProviderTimeZoneResult.Null);

    public abstract Task<ProviderTimeZoneResult> DetermineProviderTimeZoneResult(HttpContext httpContext);

    protected static Task<ProviderTimeZoneResult> CreateResult(string? value, bool trimPrefix, string prefix)
    {
        if (string.IsNullOrEmpty(value: value))
        {
            return DefaultProviderTimeZoneResult;
        }

        if (trimPrefix)
        {
            value = value.StartsWith(value: prefix, comparisonType: StringComparison.OrdinalIgnoreCase)
                ? value[prefix.Length..]
                : value;
        }

        return Task.FromResult(result: ProviderTimeZoneResult.New(name: value));
    }
}