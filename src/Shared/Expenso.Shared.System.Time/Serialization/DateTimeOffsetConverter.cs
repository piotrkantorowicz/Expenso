using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

using Expenso.Shared.System.Time.Request;

namespace Expenso.Shared.System.Time.Serialization;

internal sealed class DateTimeOffsetConverter : JsonConverter<DateTimeOffset>
{
    private readonly string _format;
    private readonly Func<RequestTimeZone> _requestTimeZone;

    public DateTimeOffsetConverter(Func<RequestTimeZone> requestTimeZone, string format)
    {
        _requestTimeZone = requestTimeZone ?? throw new ArgumentNullException(paramName: nameof(requestTimeZone));
        _format = format ?? throw new ArgumentNullException(paramName: nameof(format));
    }

    public override DateTimeOffset Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        string? value = reader.GetString();

        if (string.IsNullOrEmpty(value: value))
        {
            throw new JsonException(message: "DateTime string cannot be null or empty");
        }

        if (!DateTimeOffset.TryParse(input: value, result: out DateTimeOffset dateTimeOffset))
        {
            throw new JsonException(message: $"DateTime string '{value}' does not match expected format '{_format}'");
        }

        TimeZoneInfo timeZone = _requestTimeZone().TimeZone;

        return TimeZoneInfo
            .ConvertTime(dateTimeOffset: dateTimeOffset, destinationTimeZone: timeZone)
            .ToUniversalTime();
    }

    public override void Write(Utf8JsonWriter writer, DateTimeOffset value, JsonSerializerOptions options)
    {
        ArgumentNullException.ThrowIfNull(argument: writer);
        string dateString = value.ToString(format: _format, formatProvider: CultureInfo.InvariantCulture);

        if (!DateTimeOffset.TryParseExact(input: dateString, format: _format,
                formatProvider: CultureInfo.InvariantCulture, styles: DateTimeStyles.None,
                result: out DateTimeOffset parsedDateTimeOffset))
        {
            throw new JsonException(
                message: $"Failed to parse formatted date string '{dateString}' back to DateTimeOffset");
        }

        TimeZoneInfo? timeZone = _requestTimeZone().TimeZone;

        if (timeZone == null)
        {
            throw new InvalidOperationException(message: "TimeZone cannot be null");
        }

        try
        {
            DateTimeOffset convertTime =
                TimeZoneInfo.ConvertTime(dateTimeOffset: parsedDateTimeOffset, destinationTimeZone: timeZone);

            writer.WriteStringValue(value: convertTime);
            writer.Flush();
        }
        catch (TimeZoneNotFoundException ex)
        {
            throw new JsonException(message: "Failed to convert DateTimeOffset to the specified timezone",
                innerException: ex);
        }
    }
}