using Expenso.Shared.System.Time.Features;
using Expenso.Shared.System.Time.Features.Interfaces;
using Expenso.Shared.System.Time.Providers;
using Expenso.Shared.System.Time.Providers.Inputs;
using Expenso.Shared.System.Time.Providers.Interfaces;
using Expenso.Shared.System.Time.Request;
using Expenso.Shared.System.Time.Request.Settings;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Expenso.Shared.System.Time.Middleware;

internal sealed class RequestDateTimeFormatMiddleware : IMiddleware
{
    private readonly ILogger _logger;
    private readonly RequestTimeZoneOptions _options;
    private readonly IProviderInput _providerInput;
    private readonly ITimeZoneClock _timeZoneClock;

    public RequestDateTimeFormatMiddleware(ILoggerFactory loggerFactory, RequestTimeZoneOptions options,
        ITimeZoneClock timeZoneClock,
        [FromKeyedServices(key: RequestProviderValueType.DateTimeFormat)] IProviderInput providerInput)
    {
        ArgumentNullException.ThrowIfNull(argument: loggerFactory);

        _logger = loggerFactory.CreateLogger<RequestTimeZoneMiddleware>() ??
                  throw new ArgumentNullException(paramName: nameof(loggerFactory));

        _options = options ?? throw new ArgumentNullException(paramName: nameof(options));
        _timeZoneClock = timeZoneClock ?? throw new ArgumentNullException(paramName: nameof(timeZoneClock));
        _providerInput = providerInput ?? throw new ArgumentNullException(paramName: nameof(providerInput));
    }

    public async Task InvokeAsync(HttpContext httpContext, RequestDelegate next)
    {
        ArgumentNullException.ThrowIfNull(argument: httpContext);
        ArgumentNullException.ThrowIfNull(argument: next);
        string defaultRequestDateTimeOffsetFormat = _options.DateTimeFormat;
        IRequestProvider? usedProvider = null;

        foreach (IRequestProvider provider in _options.RequestProviders)
        {
            ProviderResult providerTimeZoneResult = await provider.DetermineProviderResult(httpContext: httpContext,
                providerInput: _providerInput.GetInput(requestProviderType: provider.ProviderType));

            try
            {
                defaultRequestDateTimeOffsetFormat = new RequestDateTimeFormat(format: providerTimeZoneResult.Value!);
                usedProvider = provider;

                break;
            }
            catch
            {
                //TODO: Define custom exception for missing provider result
            }
        }

        httpContext.Features.Set<IRequestDateTimeFormatFeature>(
            instance: new RequestDateTimeFormatFeature(requestDateTimeFormat: defaultRequestDateTimeOffsetFormat,
                provider: usedProvider));

        httpContext.Response.Headers[
            key: _options.GetDefaultProviderValue(providerInput: _providerInput,
                providerType: RequestProviderType.Header)] = defaultRequestDateTimeOffsetFormat;

        _timeZoneClock.SetDateTimeOffsetFormat(dateTimeOffsetFormat: defaultRequestDateTimeOffsetFormat);
        await next(context: httpContext);
    }
}