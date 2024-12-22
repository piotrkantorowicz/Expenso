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

        ValidateAndAddConverters(converters: options.SerializerOptions.Converters, timeZoneClock: timeZoneClock,
            datesFormat: datesFormat);

        return options;
    }

    public static Microsoft.AspNetCore.Mvc.JsonOptions AddDateTimeConverters(
        this Microsoft.AspNetCore.Mvc.JsonOptions? options, ITimeZoneClock? timeZoneClock, string datesFormat)
    {
        ArgumentNullException.ThrowIfNull(argument: options);
        ArgumentNullException.ThrowIfNull(argument: timeZoneClock);

        ValidateAndAddConverters(converters: options.JsonSerializerOptions.Converters, timeZoneClock: timeZoneClock,
            datesFormat: datesFormat);

        return options;
    }

    private static void ValidateAndAddConverters(IList<JsonConverter> converters, ITimeZoneClock timeZoneClock,
        string datesFormat)
    {
        ArgumentNullException.ThrowIfNull(argument: datesFormat);

        try
        {
            string _ = timeZoneClock.Now.ToString(format: datesFormat);
        }
        catch (FormatException ex)
        {
            throw new ArgumentException(message: "Invalid date format string", paramName: nameof(datesFormat),
                innerException: ex);
        }

        if (converters.Any(predicate: c => c is DateTimeConverter or DateTimeOffsetConverter))
        {
            return;
        }

        converters.AddTimeZoneConverters(timeZoneClock: timeZoneClock, datesFormat: datesFormat);
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