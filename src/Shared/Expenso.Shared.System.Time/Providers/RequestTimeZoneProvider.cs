using Expenso.Shared.System.Time.Constants;
using Expenso.Shared.System.Time.Providers.Interfaces;
using Expenso.Shared.System.Time.Request.Settings;

using Microsoft.AspNetCore.Http;

namespace Expenso.Shared.System.Time.Providers;

internal abstract class RequestTimeZoneProvider : IRequestTimeZoneProvider
{
    protected static readonly Task<ProviderTimeZoneResult> DefaultProviderTimeZoneResult =
        Task.FromResult(result: new ProviderTimeZoneResult(name: TimeZoneIds.Utc));

    protected RequestTimeZoneProvider(RequestTimeZoneOptions? options)
    {
        Options = options ?? throw new ArgumentNullException(paramName: nameof(options));
    }

    public RequestTimeZoneOptions Options { get; }

    public abstract Task<ProviderTimeZoneResult> DetermineProviderTimeZoneResult(HttpContext httpContext);

    protected static Task<ProviderTimeZoneResult> ValidateAndCreateResult(string? value, bool trimPrefix = true,
        string prefix = "timezone=")
    {
        if (string.IsNullOrEmpty(value: value) || !IsValidTimeZoneId(timeZoneId: value))
        {
            return DefaultProviderTimeZoneResult;
        }

        if (trimPrefix)
        {
            value = value.Replace(oldValue: prefix, newValue: "");
        }

        return Task.FromResult(result: new ProviderTimeZoneResult(name: value));
    }

    private static bool IsValidTimeZoneId(string timeZoneId)
    {
        try
        {
            TimeZoneInfo.FindSystemTimeZoneById(id: timeZoneId);

            return true;
        }
        catch (TimeZoneNotFoundException)
        {
            return false;
        }
    }
}