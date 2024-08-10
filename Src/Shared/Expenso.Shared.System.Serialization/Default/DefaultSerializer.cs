using System.Text.Json;

using Microsoft.Extensions.Logging;

namespace Expenso.Shared.System.Serialization.Default;

internal sealed class DefaultSerializer(ILogger<DefaultSerializer> logger) : ISerializer
{
    private readonly ILogger<DefaultSerializer> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    public T? Deserialize<T>(string value, object? settings = null)
    {
        switch (settings)
        {
            case null:
                return JsonSerializer.Deserialize<T>(value);
            case JsonSerializerOptions serializerOptions:
                return JsonSerializer.Deserialize<T>(value, serializerOptions);
            default:
                _logger.LogWarning("Unknown serializer options provided. Using default options");

                return JsonSerializer.Deserialize<T>(value);
        }
    }

    public object? Deserialize(string value, Type? type, object? settings = null)
    {
        if (type is null)
            return null;
        
        switch (settings)
        {
            case null:
                return JsonSerializer.Deserialize(value, type);
            case JsonSerializerOptions serializerOptions:
                return JsonSerializer.Deserialize(value, type, serializerOptions);
            default:
                _logger.LogWarning("Unknown serializer options provided. Using default options");

                return JsonSerializer.Deserialize(value, type);
        }
    }

    public string Serialize<T>(T value, object? settings = null)
    {
        switch (settings)
        {
            case null:
                return JsonSerializer.Serialize(value);
            case JsonSerializerOptions serializerOptions:
                return JsonSerializer.Serialize(value, serializerOptions);
            default:
                _logger.LogWarning("Unknown serializer options provided. Using default options");

                return JsonSerializer.Serialize(value);
        }
    }
}