using Expenso.Shared.System.Time.Providers.Inputs;
using Expenso.Shared.System.Time.Providers.Interfaces;

using Microsoft.AspNetCore.Http;

namespace Expenso.Shared.System.Time.Providers;

public abstract class RequestProvider : IRequestProvider
{
    protected static readonly Task<ProviderResult> DefaultProviderTimeZoneResult =
        Task.FromResult(result: ProviderResult.Null);

    public abstract RequestProviderType ProviderType { get; }

    public abstract Task<ProviderResult> DetermineProviderResult(HttpContext httpContext, IProviderInput providerInput);

    protected static Task<ProviderResult> CreateResult(string? value, bool trimPrefix, string prefix)
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

        return Task.FromResult(result: ProviderResult.New(value: value));
    }
}