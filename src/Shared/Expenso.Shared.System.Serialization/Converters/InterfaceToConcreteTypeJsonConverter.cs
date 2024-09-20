using System.Text.Json;
using System.Text.Json.Serialization;

namespace Expenso.Shared.System.Serialization.Converters;

public sealed class InterfaceToConcreteTypeJsonConverter<TInterface, TConcreteType> : JsonConverter<TInterface>
    where TInterface : class where TConcreteType : class, TInterface, new()
{
    public override TInterface? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions? options)
    {
        return JsonSerializer.Deserialize<TConcreteType>(reader: ref reader, options: options);
    }

    public override void Write(Utf8JsonWriter writer, TInterface value, JsonSerializerOptions options)
    {
        if (value is TConcreteType concrete)
        {
            JsonSerializer.Serialize(writer: writer, value: concrete, options: options);
        }
        else
        {
            throw new InvalidOperationException(message: "Attempted to serialize an object of incorrect type.");
        }
    }
}