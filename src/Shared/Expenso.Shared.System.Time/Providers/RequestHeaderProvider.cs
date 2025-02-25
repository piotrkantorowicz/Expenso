using Expenso.Shared.System.Time.Providers.Inputs;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace Expenso.Shared.System.Time.Providers;

public sealed class RequestHeaderProvider : RequestProvider
{
    public string? HeaderKey { get; private set; }

    public override RequestProviderType ProviderType => RequestProviderType.Header;

    public override Task<ProviderResult> DetermineProviderResult(HttpContext httpContext, IProviderInput providerInput)
    {
        ArgumentNullException.ThrowIfNull(argument: httpContext);
        ArgumentNullException.ThrowIfNull(argument: providerInput);
        HeaderKey ??= providerInput.Key;

        if (!httpContext.Request.Headers.TryGetValue(key: HeaderKey, value: out StringValues values))
        {
            return DefaultProviderTimeZoneResult;
        }

        string? value = values.FirstOrDefault(predicate: v => !string.IsNullOrEmpty(value: v));

        return CreateResult(value: value, trimPrefix: true, prefix: providerInput.Prefix);
    }
}