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

        if (value == null)
        {
            return default;
        }

        return DateTimeOffset.TryParse(input: value, result: out DateTimeOffset parsedDateTime)
            ? parsedDateTime
            : default;
    }

    public override void Write(Utf8JsonWriter writer, DateTimeOffset value, JsonSerializerOptions options)
    {
        ArgumentNullException.ThrowIfNull(argument: writer);
        string dateString = value.ToString(format: _format, formatProvider: CultureInfo.InvariantCulture);

        if (!DateTimeOffset.TryParseExact(input: dateString, format: _format,
                formatProvider: CultureInfo.InvariantCulture, styles: DateTimeStyles.None,
                result: out DateTimeOffset parsedDateTimeOffset))
        {
            return;
        }

        TimeZoneInfo timeZone = _requestTimeZone().TimeZone;

        DateTimeOffset convertTime =
            TimeZoneInfo.ConvertTime(dateTimeOffset: parsedDateTimeOffset, destinationTimeZone: timeZone);

        writer.WriteStringValue(value: convertTime);
        writer.Flush();
    }
}