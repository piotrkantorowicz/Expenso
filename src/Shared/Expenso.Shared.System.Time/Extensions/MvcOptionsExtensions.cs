using Expenso.Shared.System.Time.ModelBinders;
using Expenso.Shared.System.Time.Request;
using Expenso.Shared.System.Time.Request.Settings;

using Microsoft.AspNetCore.Mvc;

namespace Expenso.Shared.System.Time.Extensions;

public static class MvcOptionsExtensions
{
    public static MvcOptions AddDateTimeModelBinderProvider(this MvcOptions options, ITimeZoneClock timeZoneClock,
        RequestTimeZoneOptions requestTimeZoneOptions)
    {
        ArgumentNullException.ThrowIfNull(argument: timeZoneClock);
        ArgumentNullException.ThrowIfNull(argument: options);

        options.ModelBinderProviders.Insert(index: 0, item: new DateTimeModelBinderProvider(requestTimeZone: () =>
        {
            TimeZoneInfo timeZone = timeZoneClock.TimeZone ??
                                    throw new InvalidOperationException(message: "TimeZone cannot be null");

            return new RequestTimeZone(timeZoneInfo: timeZone);
        }, dateTimeSupportedFormats: requestTimeZoneOptions.SupportedDateTimeFormats,
        dateTimeOffsetSupportedFormats: requestTimeZoneOptions.SupportedDateTimeOffsetFormats));

        return options;
    }
}