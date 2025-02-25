using System.Text.Json.Serialization;

using Expenso.Shared.System.Time.Request;
using Expenso.Shared.System.Time.Serialization;

using Microsoft.AspNetCore.Http.Json;

using MvcJsonOptions = Microsoft.AspNetCore.Mvc.JsonOptions;

namespace Expenso.Shared.System.Time.Extensions;

public static class JsonOptionsExtensions
{
    public static JsonOptions AddDateTimeConverters(this JsonOptions? options, ITimeZoneClock? timeZoneClock)
    {
        ArgumentNullException.ThrowIfNull(argument: options);
        ArgumentNullException.ThrowIfNull(argument: timeZoneClock);
        ValidateAndAddConverters(converters: options.SerializerOptions.Converters, timeZoneClock: timeZoneClock);

        return options;
    }

    public static MvcJsonOptions AddDateTimeConverters(this MvcJsonOptions? options, ITimeZoneClock? timeZoneClock)
    {
        ArgumentNullException.ThrowIfNull(argument: options);
        ArgumentNullException.ThrowIfNull(argument: timeZoneClock);
        ValidateAndAddConverters(converters: options.JsonSerializerOptions.Converters, timeZoneClock: timeZoneClock);

        return options;
    }

    private static void ValidateAndAddConverters(IList<JsonConverter> converters, ITimeZoneClock timeZoneClock)
    {
        ValidateFormats(timeZoneClock: timeZoneClock,
            formats: [timeZoneClock.DateTimeFormat, timeZoneClock.DateTimeOffsetFormat]);

        if (converters.Any(predicate: c => c is DateTimeConverter or DateTimeOffsetConverter))
        {
            return;
        }

        converters.AddTimeZoneConverters(timeZoneClock: timeZoneClock);
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

    private static void AddTimeZoneConverters(this IList<JsonConverter> jsonConverters, ITimeZoneClock timeZoneClock)
    {
        Func<RequestTimeZone> requestTimeZone = () => new RequestTimeZone(timeZoneInfo: timeZoneClock.TimeZone);

        Func<RequestDateTimeFormat> requestDateTimeFormat =
            () => new RequestDateTimeFormat(format: timeZoneClock.DateTimeFormat);

        Func<RequestDateTimeOffsetFormat> requestDateTimeOffsetFormat =
            () => new RequestDateTimeOffsetFormat(format: timeZoneClock.DateTimeFormat);

        jsonConverters.AddConverters(jsonConvertersToAdd:
        [
            new DateTimeOffsetConverter(requestTimeZone: requestTimeZone,
                requestDateTimeOffsetFormat: requestDateTimeOffsetFormat),
            new DateTimeConverter(requestTimeZone: requestTimeZone, requestDateTimeFormat: requestDateTimeFormat),
            new NullableDateTimeOffsetConverter(requestTimeZone: requestTimeZone,
                requestDateTimeOffsetFormat: requestDateTimeOffsetFormat),
            new NullableDateTimeConverter(requestTimeZone: requestTimeZone,
                requestDateTimeFormat: requestDateTimeFormat)
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