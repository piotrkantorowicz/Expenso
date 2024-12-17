using Expenso.Shared.System.Time.Features;
using Expenso.Shared.System.Time.Features.Interfaces;
using Expenso.Shared.System.Time.Providers;
using Expenso.Shared.System.Time.Providers.Interfaces;
using Expenso.Shared.System.Time.Request;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Expenso.Shared.System.Time.Middleware;

internal sealed class RequestTimeZoneMiddleware : IMiddleware
{
    private readonly ILogger _logger;
    private readonly RequestTimeZoneOptions _options;
    private readonly ITimeZoneClock _timeZoneClock;

    public RequestTimeZoneMiddleware(ILoggerFactory loggerFactory, RequestTimeZoneOptions options,
        ITimeZoneClock timeZoneClock)
    {
        _logger = loggerFactory.CreateLogger<RequestTimeZoneMiddleware>() ??
                  throw new ArgumentNullException(paramName: nameof(loggerFactory));

        _options = options ?? throw new ArgumentNullException(paramName: nameof(options));
        _timeZoneClock = timeZoneClock ?? throw new ArgumentNullException(paramName: nameof(timeZoneClock));
    }

    public async Task InvokeAsync(HttpContext httpContext, RequestDelegate next)
    {
        ArgumentNullException.ThrowIfNull(argument: httpContext);
        ArgumentNullException.ThrowIfNull(argument: next);
        RequestTimeZone? requestTimeZone = _options.DefaultRequestTimeZone;
        IRequestTimeZoneProvider? usedProvider = null;

        if (_options.RequestTimeZoneProviders is not null)
        {
            foreach (IRequestTimeZoneProvider? provider in _options.RequestTimeZoneProviders)
            {
                ProviderTimeZoneResult? providerTimeZoneResult =
                    await provider.DetermineProviderTimeZoneResult(httpContext: httpContext);

                if (providerTimeZoneResult is null)
                {
                    continue;
                }

                try
                {
                    requestTimeZone = new RequestTimeZone(name: providerTimeZoneResult.TimeZoneName);
                    usedProvider = provider;

                    break;
                }
                catch (InvalidTimeZoneException ex)
                {
                    _logger.LogWarning(exception: ex, message: "Invalid TimeZone Id: {TimeZoneName}",
                        providerTimeZoneResult.TimeZoneName);
                }
                catch (TimeZoneNotFoundException ex)
                {
                    _logger.LogWarning(exception: ex, message: "TimeZone Not Found: {TimeZoneName}",
                        providerTimeZoneResult.TimeZoneName);
                }
            }
        }

        httpContext.Features.Set<IRequestTimeZoneFeature>(
            instance: new RequestTimeZoneFeature(requestTimeZone: requestTimeZone, provider: usedProvider));

        httpContext.Response.Headers[key: _options.GetDefaultHeaderName()] = requestTimeZone.TimeZone.Id;
        _timeZoneClock.SetTimeZone(timeZone: requestTimeZone.TimeZone);
        await next(context: httpContext);
    }
}