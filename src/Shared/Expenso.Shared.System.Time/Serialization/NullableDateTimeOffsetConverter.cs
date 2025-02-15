using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

using Expenso.Shared.System.Time.Request;

namespace Expenso.Shared.System.Time.Serialization;

internal sealed class NullableDateTimeOffsetConverter : JsonConverter<DateTimeOffset?>
{
    private readonly Func<RequestDateTimeOffsetFormat> _requestDateTimeOffsetFormat;
    private readonly Func<RequestTimeZone> _requestTimeZone;

    public NullableDateTimeOffsetConverter(Func<RequestTimeZone> requestTimeZone,
        Func<RequestDateTimeOffsetFormat> requestDateTimeOffsetFormat)
    {
        _requestTimeZone = requestTimeZone ?? throw new ArgumentNullException(paramName: nameof(requestTimeZone));

        _requestDateTimeOffsetFormat = requestDateTimeOffsetFormat ??
                                       throw new ArgumentNullException(paramName: nameof(requestDateTimeOffsetFormat));
    }

    public override DateTimeOffset? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        string? value = reader.GetString();

        if (string.IsNullOrEmpty(value: value))
        {
            throw new JsonException(message: "DateTime string cannot be null or empty");
        }

        RequestTimeZone requestTimeZone = _requestTimeZone();
        RequestDateTimeOffsetFormat requestDateTimeOffsetFormat = _requestDateTimeOffsetFormat();

        if (!DateTimeOffset.TryParseExact(input: value, format: requestDateTimeOffsetFormat.Format,
                formatProvider: CultureInfo.InvariantCulture, styles: DateTimeStyles.None,
                result: out DateTimeOffset dateTimeOffset))
        {
            throw new JsonException(
                message:
                $"DateTime string '{value}' does not match expected format '{requestDateTimeOffsetFormat.Format}'");
        }

        return TimeZoneInfo.ConvertTime(dateTimeOffset: dateTimeOffset, destinationTimeZone: requestTimeZone.TimeZone);
    }

    public override void Write(Utf8JsonWriter writer, DateTimeOffset? value, JsonSerializerOptions options)
    {
        if (value.HasValue is false)
        {
            writer.WriteNullValue();

            return;
        }

        try
        {
            DateTimeOffset convertTime = TimeZoneInfo.ConvertTime(dateTimeOffset: value.Value,
                destinationTimeZone: _requestTimeZone().TimeZone);

            writer.WriteStringValue(value: convertTime.ToString(format: _requestDateTimeOffsetFormat().Format,
                formatProvider: CultureInfo.InvariantCulture));

            writer.Flush();
        }
        catch (Exception ex)
        {
            throw new JsonException(message: "Failed to convert DateTimeOffset to the specified timezone",
                innerException: ex);
        }
    }
}