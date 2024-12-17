using Expenso.Shared.System.Time.ModelBinders;
using Expenso.Shared.System.Time.Request;

using Microsoft.AspNetCore.Mvc;

namespace Expenso.Shared.System.Time.Extensions;

public static class MvcOptionsExtensions
{
    public static MvcOptions AddDateTimeModelBinderProvider(this MvcOptions options, ITimeZoneClock timeZoneClock)
    {
        ArgumentNullException.ThrowIfNull(argument: timeZoneClock);
        ArgumentNullException.ThrowIfNull(argument: options);

        options.ModelBinderProviders.Insert(index: 0,
            item: new DateTimeModelBinderProvider(requestTimeZone: () =>
                new RequestTimeZone(timeZoneInfo: timeZoneClock.TimeZone)));

        return options;
    }
}