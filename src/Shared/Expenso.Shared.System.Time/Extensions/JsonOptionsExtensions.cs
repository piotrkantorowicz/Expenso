using System.Text.Json.Serialization;

using Expenso.Shared.System.Time.Request;
using Expenso.Shared.System.Time.Serialization;

using Microsoft.AspNetCore.Http.Json;

namespace Expenso.Shared.System.Time.Extensions;

public static class JsonOptionsExtensions
{
    public static JsonOptions AddDateTimeConverters(this JsonOptions? options, ITimeZoneClock? timeZoneClock,
        string datesFormat)
    {
        ArgumentNullException.ThrowIfNull(argument: options);
        ArgumentNullException.ThrowIfNull(argument: timeZoneClock);

        options.SerializerOptions.Converters.AddTimeZoneConverters(timeZoneClock: timeZoneClock,
            datesFormat: datesFormat);

        return options;
    }

    public static Microsoft.AspNetCore.Mvc.JsonOptions AddDateTimeConverters(
        this Microsoft.AspNetCore.Mvc.JsonOptions? options, ITimeZoneClock? timeZoneClock, string datesFormat)
    {
        ArgumentNullException.ThrowIfNull(argument: options);
        ArgumentNullException.ThrowIfNull(argument: timeZoneClock);

        options.JsonSerializerOptions.Converters.AddTimeZoneConverters(timeZoneClock: timeZoneClock,
            datesFormat: datesFormat);

        return options;
    }

    private static void AddTimeZoneConverters(this IList<JsonConverter> jsonConverters, ITimeZoneClock timeZoneClock,
        string datesFormat)
    {
        jsonConverters.AddConverters(jsonConvertersToAdd:
        [
            new DateTimeOffsetConverter(
                requestTimeZone: () => new RequestTimeZone(timeZoneInfo: timeZoneClock.TimeZone), format: datesFormat),
            new DateTimeConverter(requestTimeZone: () => new RequestTimeZone(timeZoneInfo: timeZoneClock.TimeZone),
                format: datesFormat),
            new NullableDateTimeOffsetConverter(
                requestTimeZone: () => new RequestTimeZone(timeZoneInfo: timeZoneClock.TimeZone), format: datesFormat),
            new NullableDateTimeConverter(
                requestTimeZone: () => new RequestTimeZone(timeZoneInfo: timeZoneClock.TimeZone), format: datesFormat)
        ]);
    }

    private static void AddConverters(this IList<JsonConverter> jsonConverters,
        IEnumerable<JsonConverter> jsonConvertersToAdd)
    {
        foreach (JsonConverter? converter in jsonConvertersToAdd)
        {
            jsonConverters.Add(item: converter);
        }
    }
}