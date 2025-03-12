using Expenso.Shared.System.Time.Features;
using Expenso.Shared.System.Time.Features.Interfaces;
using Expenso.Shared.System.Time.Providers;
using Expenso.Shared.System.Time.Providers.Interfaces;
using Expenso.Shared.System.Time.Request;
using Expenso.Shared.System.Time.Request.Settings;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Expenso.Shared.System.Time.Middleware;

internal sealed class RequestTimeZoneMiddleware
{
    private readonly ILogger _logger;
    private readonly ITimeZoneClock _timeZoneClock;
    private readonly RequestTimeZoneOptions _options;
    private readonly RequestDelegate _next;

    public RequestTimeZoneMiddleware(ILoggerFactory loggerFactory, RequestTimeZoneOptions options,
        ITimeZoneClock timeZoneClock, RequestDelegate next)
    {
        ArgumentNullException.ThrowIfNull(argument: loggerFactory);
        _next = next ?? throw new ArgumentNullException(paramName: nameof(next));

        _logger = loggerFactory.CreateLogger<RequestTimeZoneMiddleware>() ??
                  throw new ArgumentNullException(paramName: nameof(loggerFactory));

        _options = options ?? throw new ArgumentNullException(paramName: nameof(options));
        _timeZoneClock = timeZoneClock ?? throw new ArgumentNullException(paramName: nameof(timeZoneClock));
    }

    public async Task InvokeAsync(HttpContext context)
    {
        ArgumentNullException.ThrowIfNull(argument: context);
        RequestTimeZone defaultRequestTimeZone = _options.DefaultRequestTimeZone;
        IRequestTimeZoneProvider? usedProvider = null;

        foreach (IRequestTimeZoneProvider? provider in _options.RequestTimeZoneProviders)
        {
            ProviderTimeZoneResult providerTimeZoneResult =
                await provider.DetermineProviderTimeZoneResult(httpContext: context);

            try
            {
                defaultRequestTimeZone = new RequestTimeZone(name: providerTimeZoneResult.TimeZoneName);
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

        string timeZoneId = defaultRequestTimeZone.TimeZone.Id;
        string headerName = _options.GetDefaultHeaderName();

        context.Features.Set<IRequestTimeZoneFeature>(
            instance: new RequestTimeZoneFeature(requestTimeZone: defaultRequestTimeZone, provider: usedProvider));

        context.Response.OnStarting(callback: () =>
        {
            context.Response.Headers[key: headerName] = timeZoneId;

            return Task.CompletedTask;
        });

        _timeZoneClock.SetTimeZone(timeZone: defaultRequestTimeZone.TimeZone);
        await _next.Invoke(context: context);
    }
}