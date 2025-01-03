using Expenso.Shared.System.Time.Middleware;
using Expenso.Shared.System.Time.Request.Settings;

using Microsoft.Extensions.DependencyInjection;

using JsonOptions = Microsoft.AspNetCore.Http.Json.JsonOptions;

namespace Expenso.Shared.System.Time.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddClock(this IServiceCollection services, out Clock clock)
    {
        ArgumentNullException.ThrowIfNull(argument: services);
        clock = new Clock();
        services.AddSingleton<IClock>(implementationInstance: clock);
        services.AddSingleton<ITimeZoneClock>(implementationInstance: clock);

        return services;
    }

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
        RequestTimeZoneOptions options = new();
        optionsAction.Invoke(obj: options);
        services.AddSingleton(implementationFactory: _ => options).AddSingleton<RequestTimeZoneMiddleware>();

        if (options.EnableRequestToUtc)
        {
            ConfigureRequestOptions(services: services, timeZoneClock: timeZoneClock, requestTimeZoneOptions: options);
        }

        if (options.EnableResponseToLocal)
        {
            ConfigureResponseOptions(services: services, requestTimeZoneOptions: options, timeZoneClock: timeZoneClock);
        }

        return services;
    }

    private static void ConfigureRequestOptions(IServiceCollection services, ITimeZoneClock timeZoneClock,
        RequestTimeZoneOptions requestTimeZoneOptions)
    {
        services.AddMvcCore(setupAction: x =>
            x.AddDateTimeModelBinderProvider(timeZoneClock: timeZoneClock,
                requestTimeZoneOptions: requestTimeZoneOptions));
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
                ConfigureJson();

                break;
            case MvcOptionType.Controllers:
                ConfigureMvc();

                break;
            case MvcOptionType.All:
                ConfigureMvc();
                ConfigureJson();

                break;
            default:
                throw new ArgumentOutOfRangeException(paramName: nameof(requestTimeZoneOptions.MvcOptionType),
                    actualValue: requestTimeZoneOptions.MvcOptionType,
                    message: $"Unsupported MVC option type: {requestTimeZoneOptions.MvcOptionType}");
        }

        return;

        void ConfigureJson()
        {
            services.Configure<JsonOptions>(configureOptions: x => x.AddDateTimeConverters(timeZoneClock: timeZoneClock,
                dateTimeFormats: requestTimeZoneOptions.SupportedDateTimeFormats,
                dateTimeOffsetFormats: requestTimeZoneOptions.SupportedDateTimeOffsetFormats));
        }

        void ConfigureMvc()
        {
            services.AddMvcCore(setupAction: x => x.AddDateTimeModelBinderProvider(
                timeZoneClock: timeZoneClock, requestTimeZoneOptions: requestTimeZoneOptions));
        }
    }
}