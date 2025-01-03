using System.Text.Json.Serialization;

using Expenso.Shared.System.Time.Request;
using Expenso.Shared.System.Time.Serialization;

using Microsoft.AspNetCore.Http.Json;

namespace Expenso.Shared.System.Time.Extensions;

public static class JsonOptionsExtensions
{
    public static JsonOptions AddDateTimeConverters(this JsonOptions? options, ITimeZoneClock? timeZoneClock,
        string[] dateTimeFormats, string[] dateTimeOffsetFormats)
    {
        ArgumentNullException.ThrowIfNull(argument: options);
        ArgumentNullException.ThrowIfNull(argument: timeZoneClock);

        ValidateAndAddConverters(converters: options.SerializerOptions.Converters, timeZoneClock: timeZoneClock,
            dateTimeFormats: dateTimeFormats, dateTimeOffsetFormats: dateTimeOffsetFormats);

        return options;
    }

    public static Microsoft.AspNetCore.Mvc.JsonOptions AddDateTimeConverters(
        this Microsoft.AspNetCore.Mvc.JsonOptions? options, ITimeZoneClock? timeZoneClock, string[] dateTimeFormats,
        string[] dateTimeOffsetFormats)
    {
        ArgumentNullException.ThrowIfNull(argument: options);
        ArgumentNullException.ThrowIfNull(argument: timeZoneClock);

        ValidateAndAddConverters(converters: options.JsonSerializerOptions.Converters, timeZoneClock: timeZoneClock,
            dateTimeFormats: dateTimeFormats, dateTimeOffsetFormats: dateTimeOffsetFormats);

        return options;
    }

    private static void ValidateAndAddConverters(IList<JsonConverter> converters, ITimeZoneClock timeZoneClock,
        string[] dateTimeFormats, string[] dateTimeOffsetFormats)
    {
        ValidateFormats(timeZoneClock: timeZoneClock, formats: dateTimeFormats);
        ValidateFormats(timeZoneClock: timeZoneClock, formats: dateTimeOffsetFormats);

        if (converters.Any(predicate: c => c is DateTimeConverter or DateTimeOffsetConverter))
        {
            return;
        }

        converters.AddTimeZoneConverters(timeZoneClock: timeZoneClock, dateTimeFormats: dateTimeFormats,
            dateTimeOffsetFormats: dateTimeOffsetFormats);
    }

    private static void ValidateFormats(ITimeZoneClock timeZoneClock, string[] formats)
    {
        foreach (string? format in formats)
        {
            ValidateFormat(timeZoneClock: timeZoneClock, format: format);
        }
    }

    private static void ValidateFormat(ITimeZoneClock timeZoneClock, string format)
    {
        ArgumentNullException.ThrowIfNull(argument: format);

        try
        {
            string _ = timeZoneClock.Now.ToString(format: format);
        }
        catch (FormatException ex)
        {
            throw new ArgumentException(message: "Invalid date format string", paramName: nameof(format),
                innerException: ex);
        }
    }

    private static void AddTimeZoneConverters(this IList<JsonConverter> jsonConverters, ITimeZoneClock timeZoneClock,
        string[] dateTimeFormats, string[] dateTimeOffsetFormats)
    {
        jsonConverters.AddConverters(jsonConvertersToAdd:
        [
            new DateTimeOffsetConverter(
                requestTimeZone: () => new RequestTimeZone(timeZoneInfo: timeZoneClock.TimeZone),
                supportedFormats: dateTimeOffsetFormats),
            new DateTimeConverter(requestTimeZone: () => new RequestTimeZone(timeZoneInfo: timeZoneClock.TimeZone),
                supportedFormats: dateTimeFormats),
            new NullableDateTimeOffsetConverter(
                requestTimeZone: () => new RequestTimeZone(timeZoneInfo: timeZoneClock.TimeZone),
                supportedFormats: dateTimeOffsetFormats),
            new NullableDateTimeConverter(
                requestTimeZone: () => new RequestTimeZone(timeZoneInfo: timeZoneClock.TimeZone),
                supportedFormats: dateTimeFormats)
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