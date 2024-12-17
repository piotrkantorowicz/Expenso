using Expenso.Shared.System.Time.Configuration;
using Expenso.Shared.System.Time.Middleware;
using Expenso.Shared.System.Time.Request;

using Microsoft.Extensions.DependencyInjection;

using JsonOptions = Microsoft.AspNetCore.Http.Json.JsonOptions;

namespace Expenso.Shared.System.Time.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddRequestTimeZone(this IServiceCollection services, string defaultTimeZone,
        ITimeZoneClock timeZoneClock)
    {
        ArgumentNullException.ThrowIfNull(argument: services);

        return services.AddRequestTimeZone(optionsAction: x => x.Id = defaultTimeZone, timeZoneClock: timeZoneClock);
    }

    public static IServiceCollection AddRequestTimeZone(this IServiceCollection services,
        Action<RequestTimeZoneOptions> optionsAction, ITimeZoneClock timeZoneClock)
    {
        ArgumentNullException.ThrowIfNull(argument: services);
        ArgumentNullException.ThrowIfNull(argument: optionsAction);
        ArgumentNullException.ThrowIfNull(argument: timeZoneClock);
        RequestTimeZoneOptions? options = new();
        optionsAction.Invoke(obj: options);
        services.AddSingleton(implementationFactory: _ => options).AddSingleton<RequestTimeZoneMiddleware>();

        if (options.EnableRequestToUtc)
        {
            ConfigureRequestOptions(services: services, timeZoneClock: timeZoneClock);
        }

        if (options.EnableResponseToLocal)
        {
            ConfigureResponseOptions(services: services, requestTimeZoneOptions: options, timeZoneClock: timeZoneClock);
        }

        return services;
    }

    private static void ConfigureRequestOptions(IServiceCollection services, ITimeZoneClock timeZoneClock)
    {
        services.AddMvcCore(setupAction: x => x.AddDateTimeModelBinderProvider(timeZoneClock: timeZoneClock));
    }

    private static void ConfigureResponseOptions(IServiceCollection services,
        RequestTimeZoneOptions requestTimeZoneOptions, ITimeZoneClock timeZoneClock)
    {
        switch (requestTimeZoneOptions.MvcOptionType)
        {
            case MvcOptionType.None:
                break;
            case MvcOptionType.MinimalApi:
                services.AddMvcCore();

                services.Configure<JsonOptions>(configureOptions: x =>
                    x.AddDateTimeConverters(timeZoneClock: timeZoneClock,
                        datesFormat: requestTimeZoneOptions.DatesFormat));

                break;
            case MvcOptionType.Controllers:
                services.AddMvcCore(setupAction: x => x.AddDateTimeModelBinderProvider(timeZoneClock: timeZoneClock));

                break;
            case MvcOptionType.All:
                services.AddMvcCore(setupAction: x => x.AddDateTimeModelBinderProvider(timeZoneClock: timeZoneClock));

                services.Configure<JsonOptions>(configureOptions: x =>
                    x.AddDateTimeConverters(timeZoneClock: timeZoneClock,
                        datesFormat: requestTimeZoneOptions.DatesFormat));

                break;
            default:
                throw new ArgumentOutOfRangeException(paramName: nameof(requestTimeZoneOptions.MvcOptionType),
                    actualValue: requestTimeZoneOptions.MvcOptionType, message: null);
        }
    }
}