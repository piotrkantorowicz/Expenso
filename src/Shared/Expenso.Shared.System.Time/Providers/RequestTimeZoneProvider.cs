using Expenso.Shared.System.Time.Providers.Interfaces;
using Expenso.Shared.System.Time.Request;

using Microsoft.AspNetCore.Http;

namespace Expenso.Shared.System.Time.Providers;

internal abstract class RequestTimeZoneProvider : IRequestTimeZoneProvider
{
    protected static readonly Task<ProviderTimeZoneResult?> NullProviderTimeZoneResult =
        Task.FromResult(result: default(ProviderTimeZoneResult));

    protected RequestTimeZoneProvider(RequestTimeZoneOptions? options)
    {
        Options = options ?? throw new ArgumentNullException(paramName: nameof(options));
    }

    public RequestTimeZoneOptions Options { get; }

    public abstract Task<ProviderTimeZoneResult?> DetermineProviderTimeZoneResult(HttpContext httpContext);
}