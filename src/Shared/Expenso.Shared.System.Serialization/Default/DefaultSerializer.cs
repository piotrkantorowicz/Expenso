using System.Text.Json;

using Expenso.Shared.System.Logging;

namespace Expenso.Shared.System.Serialization.Default;

internal sealed class DefaultSerializer : ISerializer
{
    private readonly ILoggerService<DefaultSerializer> _logger;

    public DefaultSerializer(ILoggerService<DefaultSerializer> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(paramName: nameof(logger));
    }

    public T? Deserialize<T>(string value, object? settings = null)
    {
        switch (settings)
        {
            case null:
                return JsonSerializer.Deserialize<T>(json: value);
            case JsonSerializerOptions serializerOptions:
                return JsonSerializer.Deserialize<T>(json: value, options: serializerOptions);
            default:
                _logger.LogWarning(eventId: LoggingUtils.SerializationWarning,
                    message: "Unknown serializer options provided. Using default options");

                return JsonSerializer.Deserialize<T>(json: value);
        }
    }

    public object? Deserialize(string value, Type? type, object? settings = null)
    {
        if (type is null)
        {
            return null;
        }

        switch (settings)
        {
            case null:
                return JsonSerializer.Deserialize(json: value, returnType: type);
            case JsonSerializerOptions serializerOptions:
                return JsonSerializer.Deserialize(json: value, returnType: type, options: serializerOptions);
            default:
                _logger.LogWarning(eventId: LoggingUtils.SerializationWarning,
                    message: "Unknown serializer options provided. Using default options");

                return JsonSerializer.Deserialize(json: value, returnType: type);
        }
    }

    public string Serialize<T>(T value, object? settings = null)
    {
        switch (settings)
        {
            case null:
                return JsonSerializer.Serialize(value: value);
            case JsonSerializerOptions serializerOptions:
                return JsonSerializer.Serialize(value: value, options: serializerOptions);
            default:
                _logger.LogWarning(eventId: LoggingUtils.SerializationWarning,
                    message: "Unknown serializer options provided. Using default options");

                return JsonSerializer.Serialize(value: value);
        }
    }
}