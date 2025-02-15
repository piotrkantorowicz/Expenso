using Expenso.Shared.System.Time.Providers.Inputs;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace Expenso.Shared.System.Time.Providers;

public sealed class RequestQueryStringProvider : RequestProvider
{
    public string? QueryStringKey { get; private set; }

    public override RequestProviderType ProviderType => RequestProviderType.QueryString;

    public override Task<ProviderResult> DetermineProviderResult(HttpContext httpContext, IProviderInput providerInput)
    {
        ArgumentNullException.ThrowIfNull(argument: httpContext);
        ArgumentNullException.ThrowIfNull(argument: providerInput);
        QueryStringKey ??= providerInput.Key;

        if (!httpContext.Request.Query.TryGetValue(key: QueryStringKey, value: out StringValues values))
        {
            return DefaultProviderTimeZoneResult;
        }

        string? value = values.FirstOrDefault(predicate: v => !string.IsNullOrEmpty(value: v));

        return CreateResult(value: value, trimPrefix: true, prefix: providerInput.Prefix);
    }
}